using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Messaging.Api.Contracts;
using Messaging.Api.Models;

namespace Messaging.Api.Tests.Integration;

public sealed class MessageStatusFlowTests(MessagingApiFactory factory) : IClassFixture<MessagingApiFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = CreateJsonOptions();

    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task FullFlow_CreatesUsers_SendsMessage_AcknowledgesDelivery_AndReturnsUpdatedHistory()
    {
        var sender = await CreateUserAsync("alice");
        var receiver = await CreateUserAsync("bob");

        using var sendResponse = await client.PostAsJsonAsync(
            "/api/messages",
            new SendMessageRequest(sender.Id, receiver.Id, "Hello Bob"),
            JsonOptions);

        Assert.Equal(HttpStatusCode.Created, sendResponse.StatusCode);

        var sentMessage = await sendResponse.Content.ReadFromJsonAsync<MessageResponse>(JsonOptions);
        Assert.NotNull(sentMessage);
        Assert.Equal(MessageStatus.Sent, sentMessage.Status);

        using var acknowledgeDeliveryRequest = new HttpRequestMessage(
            HttpMethod.Patch,
            $"/api/messages/{sentMessage.MessageId}/status")
        {
            Content = JsonContent.Create(
                new UpdateMessageStatusRequest(MessageStatus.Delivered),
                options: JsonOptions)
        };

        using var acknowledgeDeliveryResponse = await client.SendAsync(acknowledgeDeliveryRequest);

        Assert.Equal(HttpStatusCode.OK, acknowledgeDeliveryResponse.StatusCode);

        var deliveredMessage = await acknowledgeDeliveryResponse.Content.ReadFromJsonAsync<MessageResponse>(JsonOptions);
        Assert.NotNull(deliveredMessage);
        Assert.Equal(MessageStatus.Delivered, deliveredMessage.Status);

        var history = await client.GetFromJsonAsync<IReadOnlyList<MessageResponse>>(
            $"/api/conversations/{sender.Id}",
            JsonOptions);

        Assert.NotNull(history);

        var storedMessage = Assert.Single(history);
        Assert.Equal(sentMessage.MessageId, storedMessage.MessageId);
        Assert.Equal(sender.Id, storedMessage.SenderId);
        Assert.Equal(receiver.Id, storedMessage.ReceiverId);
        Assert.Equal("Hello Bob", storedMessage.Content);
        Assert.Equal(MessageStatus.Delivered, storedMessage.Status);
    }

    private async Task<UserResponse> CreateUserAsync(string username)
    {
        using var response = await client.PostAsJsonAsync(
            "/api/users",
            new CreateUserRequest(username),
            JsonOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserResponse>(JsonOptions);
        Assert.NotNull(user);

        return user;
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
