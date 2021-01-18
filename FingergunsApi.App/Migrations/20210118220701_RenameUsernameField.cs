using Microsoft.EntityFrameworkCore.Migrations;

namespace FingergunsApi.App.Migrations
{
    public partial class RenameUsernameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Username", "Users", "DisplayName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("DisplayName", "Users", "Username");
        }
    }
}
