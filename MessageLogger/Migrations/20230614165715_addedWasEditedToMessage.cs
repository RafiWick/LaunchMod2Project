using Microsoft.EntityFrameworkCore.Migrations;


#nullable disable

namespace MessageLogger.Migrations
{
    /// <inheritdoc />
    public partial class addedWasEditedToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "was_edited",
                table: "messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "was_edited",
                table: "messages");
        }
    }
}
