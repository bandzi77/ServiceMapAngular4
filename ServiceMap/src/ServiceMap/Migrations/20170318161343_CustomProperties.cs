using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ServiceMap.Migrations
{
    public partial class CustomProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LimitOfRequestsPerDay",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRequestsPerDay",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "_Id",
                table: "AspNetUsers",
                nullable: false).
                Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "_IdIndex",
                column: "_Id",
                table: "AspNetUsers",
                unique:true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitOfRequestsPerDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumberOfRequestsPerDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "_Id",
                table: "AspNetUsers");
        }
    }
}
