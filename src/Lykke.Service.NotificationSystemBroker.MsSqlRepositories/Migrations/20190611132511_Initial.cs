using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.NotificationSystemBroker.MsSqlRepositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification_system_broker");

            migrationBuilder.CreateTable(
                name: "email_messages",
                schema: "notification_system_broker",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    email = table.Column<string>(maxLength: 320, nullable: true),
                    message_id = table.Column<Guid>(nullable: false),
                    subject = table.Column<string>(maxLength: 1000, nullable: true),
                    body = table.Column<string>(nullable: true),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_messages", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "email_messages",
                schema: "notification_system_broker");
        }
    }
}
