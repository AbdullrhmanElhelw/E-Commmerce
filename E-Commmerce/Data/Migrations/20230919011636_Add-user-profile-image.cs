using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commmerce.Migrations
{
    /// <inheritdoc />
    public partial class Adduserprofileimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Users");
        }
    }
}
