using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IVForum.API.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "TotalMoney",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Forums",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Forums");

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Projects",
                nullable: true);
        }
    }
}
