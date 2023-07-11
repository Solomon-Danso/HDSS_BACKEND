﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sjhdfbarbfjbfsjcsdjfhcvdvcbasdvh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreIncreaserToken",
                table: "ClassScores");

            migrationBuilder.DropColumn(
                name: "SumToken",
                table: "ClassScores");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "ClassScores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScoreIncreaserToken",
                table: "ClassScores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SumToken",
                table: "ClassScores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalScore",
                table: "ClassScores",
                type: "float",
                nullable: true);
        }
    }
}
