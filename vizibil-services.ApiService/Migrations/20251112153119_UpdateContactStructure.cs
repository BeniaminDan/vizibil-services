using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vizibilservices.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Contacts",
                newName: "Service");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Contacts",
                newName: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Budget",
                table: "Contacts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Contacts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Contacts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Contacts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "Service",
                table: "Contacts",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Contacts",
                newName: "Content");
        }
    }
}
