using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.NotificationSystemBroker.MsSqlRepositories.Migrations
{
    public partial class AddedIndexOnEmailMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_email_messages_email",
                schema: "notification_system_broker",
                table: "email_messages",
                column: "email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_email_messages_email",
                schema: "notification_system_broker",
                table: "email_messages");
        }
    }
}
