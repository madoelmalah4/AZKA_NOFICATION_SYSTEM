using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Azka.NotificationPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationNameAndProviderSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "Notifications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationName",
                table: "Notifications",
                column: "ApplicationName");

            migrationBuilder.InsertData(
                table: "NotificationProviders",
                columns: new[] { "ProviderId", "Channel", "IsActive", "ProviderName" },
                values: new object[,]
                {
                    { new Guid("b1111111-1111-1111-1111-111111111111"), 0, true, "SendGrid" },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), 1, true, "Twilio" },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), 2, true, "Firebase" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationProviders",
                keyColumn: "ProviderId",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "NotificationProviders",
                keyColumn: "ProviderId",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "NotificationProviders",
                keyColumn: "ProviderId",
                keyValue: new Guid("b3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ApplicationName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "Notifications");
        }
    }
}
