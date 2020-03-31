using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace chat_app.domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    PasswordHash = table.Column<byte[]>(maxLength: 64, nullable: true),
                    PasswordSalt = table.Column<byte[]>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_Name",
                table: "ChatUsers",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUsers");
        }
    }
}
