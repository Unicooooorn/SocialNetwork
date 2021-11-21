using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SocialNetwork.Migrations.MessageDb
{
    public partial class MessageDbV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "MessageDb",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageDb",
                table: "MessageDb",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDb_Id",
                table: "MessageDb",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageDb",
                table: "MessageDb");

            migrationBuilder.DropIndex(
                name: "IX_MessageDb_Id",
                table: "MessageDb");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MessageDb");
        }
    }
}
