using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoldovaDentAPI.Migrations
{
    public partial class ProfileSecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncorrectAttempts",
                table: "Profiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncorrectAttempts",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "Profiles");
        }
    }
}
