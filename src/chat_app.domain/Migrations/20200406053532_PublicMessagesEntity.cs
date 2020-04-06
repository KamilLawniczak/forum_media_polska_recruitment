using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace chat_app.domain.Migrations
{
    public partial class PublicMessagesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    When = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicMessages_When",
                table: "PublicMessages",
                column: "When");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicMessages");
        }
    }
}
