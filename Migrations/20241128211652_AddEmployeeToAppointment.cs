using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShopManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Salons_SalonId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SalonId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SalonId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Expertise",
                table: "Employees",
                newName: "Specialty");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Employees",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Employees",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Specialty",
                table: "Employees",
                newName: "Expertise");

            migrationBuilder.AddColumn<int>(
                name: "SalonId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SalonId",
                table: "Employees",
                column: "SalonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Salons_SalonId",
                table: "Employees",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
