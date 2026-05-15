using System.Text.Json.Serialization;
using Messaging.Api.Data;
using Messaging.Api.Middleware;
using Messaging.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<MessagingDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=messaging.db";

    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();

var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MessagingDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();

public partial class Program;
