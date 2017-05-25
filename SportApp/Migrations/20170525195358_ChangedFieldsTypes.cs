using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportApp.Migrations
{
    public partial class ChangedFieldsTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Gym",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("MySql:ValueGeneratedOnAdd", true)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<int>(
                name: "GymId",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GymId",
                table: "Comments",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Gym_GymId",
                table: "Comments",
                column: "GymId",
                principalTable: "Gym",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Gym_GymId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_GymId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Gym",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGeneratedOnAdd", true)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "GymId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
