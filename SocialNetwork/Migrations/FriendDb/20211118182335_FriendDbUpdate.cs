using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetwork.Migrations.FriendDb
{
    public partial class FriendDbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendDb",
                columns: table => new
                {
                    MyAccount = table.Column<long>(type: "bigint", nullable: false),
                    FriendAccount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendDb");
        }
    }
}
