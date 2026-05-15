using Messaging.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messaging.Api.Data.Migrations;

[DbContext(typeof(MessagingDbContext))]
[Migration("20260514183700_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", user => user.Id);
            });

        migrationBuilder.CreateTable(
            name: "Conversations",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Type = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                UserAId = table.Column<int>(type: "INTEGER", nullable: false),
                UserBId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Conversations", conversation => conversation.Id);
                table.ForeignKey(
                    name: "FK_Conversations_Users_UserAId",
                    column: conversation => conversation.UserAId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Conversations_Users_UserBId",
                    column: conversation => conversation.UserBId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                ConversationId = table.Column<int>(type: "INTEGER", nullable: false),
                SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                ReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                Content = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                Timestamp = table.Column<long>(type: "INTEGER", nullable: false),
                Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", message => message.MessageId);
                table.ForeignKey(
                    name: "FK_Messages_Conversations_ConversationId",
                    column: message => message.ConversationId,
                    principalTable: "Conversations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Messages_Users_ReceiverId",
                    column: message => message.ReceiverId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Messages_Users_SenderId",
                    column: message => message.SenderId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ReceiverId",
            table: "Messages",
            column: "ReceiverId");

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_UserAId_UserBId",
            table: "Conversations",
            columns: new[] { "UserAId", "UserBId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_UserBId",
            table: "Conversations",
            column: "UserBId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ConversationId_Timestamp",
            table: "Messages",
            columns: new[] { "ConversationId", "Timestamp" });

        migrationBuilder.CreateIndex(
            name: "IX_Messages_SenderId_ReceiverId_Timestamp",
            table: "Messages",
            columns: new[] { "SenderId", "ReceiverId", "Timestamp" });

        migrationBuilder.CreateIndex(
            name: "IX_Users_Username",
            table: "Users",
            column: "Username",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Messages");
        migrationBuilder.DropTable(name: "Conversations");
        migrationBuilder.DropTable(name: "Users");
    }
}
