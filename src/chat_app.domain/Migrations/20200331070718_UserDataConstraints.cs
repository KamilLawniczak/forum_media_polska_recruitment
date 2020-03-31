using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace chat_app.domain.Migrations
{
    public partial class UserDataConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatUsers_Name",
                table: "ChatUsers");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "ChatUsers",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "ChatUsers",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChatUsers",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_Name",
                table: "ChatUsers",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatUsers_Name",
                table: "ChatUsers");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "ChatUsers",
                type: "varbinary(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "ChatUsers",
                type: "varbinary(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChatUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_Name",
                table: "ChatUsers",
                column: "Name");
        }
    }
}
