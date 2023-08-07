using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebhookSender.Migrations
{
    /// <inheritdoc />
    public partial class Webhook_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "WebhookEndpoints",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WebhookEndpoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WebhookEndpoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WebhookEndpoints");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WebhookEndpoints");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "WebhookEndpoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
