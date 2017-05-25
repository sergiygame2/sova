using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportApp.Migrations
{
    public partial class FirstModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CommentText = table.Column<string>(nullable: true),
                    GymId = table.Column<string>(nullable: true),
                    PublicationDate = table.Column<DateTime>(nullable: false),
                    Rate = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gym",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    Facilities = table.Column<string>(nullable: true),
                    FoundYear = table.Column<int>(nullable: false),
                    GoogleLocation = table.Column<string>(nullable: true),
                    GymArea = table.Column<int>(nullable: false),
                    GymImgUrl = table.Column<string>(nullable: true),
                    GymLocation = table.Column<string>(nullable: true),
                    GymName = table.Column<string>(nullable: true),
                    GymRate = table.Column<int>(nullable: false),
                    MbrshipPrice = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gym", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Gym");
        }
    }
}
