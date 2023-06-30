using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registrationform.Migrations
{
    public partial class AddedProfilePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoUrl",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoUrl",
                table: "UserDetails");
        }
    }
}
