using Messaging.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Api.Data;

public sealed class MessagingDbContext(DbContextOptions<MessagingDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Username)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(user => user.Username)
                .IsUnique();
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(conversation => conversation.Id);

            entity.Property(conversation => conversation.Type)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(conversation => conversation.UserAId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(conversation => conversation.UserBId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(conversation => new
            {
                conversation.UserAId,
                conversation.UserBId
            }).IsUnique();
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(message => message.MessageId);

            entity.Property(message => message.Content)
                .HasMaxLength(2_000)
                .IsRequired();

            entity.Property(message => message.Timestamp)
                .HasConversion(
                    timestamp => timestamp.ToUnixTimeMilliseconds(),
                    milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(milliseconds))
                .IsRequired();

            entity.Property(message => message.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.HasOne(message => message.Sender)
                .WithMany(user => user.SentMessages)
                .HasForeignKey(message => message.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(message => message.Receiver)
                .WithMany(user => user.ReceivedMessages)
                .HasForeignKey(message => message.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(message => message.Conversation)
                .WithMany(conversation => conversation.Messages)
                .HasForeignKey(message => message.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(message => new
            {
                message.ConversationId,
                message.Timestamp
            });

            entity.HasIndex(message => new
            {
                message.SenderId,
                message.ReceiverId,
                message.Timestamp
            });
        });
    }
}
