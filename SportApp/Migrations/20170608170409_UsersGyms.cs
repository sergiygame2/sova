using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportApp.Migrations
{
    public partial class UsersGyms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersGyms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    GymId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersGyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersGyms_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersGyms_Gym_GymId",
                        column: x => x.GymId,
                        principalTable: "Gym",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersGyms_ApplicationUserId",
                table: "UsersGyms",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGyms_GymId",
                table: "UsersGyms",
                column: "GymId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersGyms");
        }
    }
}
