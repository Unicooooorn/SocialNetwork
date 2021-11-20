using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SocialNetwork.Migrations
{
    public partial class RedesignDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<List<long>>(
                name: "Friend",
                table: "AccountDb",
                type: "bigint[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friend",
                table: "AccountDb");

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    FriendId = table.Column<long>(type: "bigint", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Login = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_AccountDb_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AccountDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profile_Profile_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AccountId",
                table: "Profile",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_FriendId",
                table: "Profile",
                column: "FriendId");
        }
    }
}
