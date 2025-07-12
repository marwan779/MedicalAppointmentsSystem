using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentsSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAppointmentRequest4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userAppointmentRequests_AspNetUsers_UserId",
                table: "userAppointmentRequests");

            migrationBuilder.DropIndex(
                name: "IX_userAppointmentRequests_UserId",
                table: "userAppointmentRequests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "userAppointmentRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "PreferredDate",
                table: "userAppointmentRequests",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredDate",
                table: "userAppointmentRequests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "userAppointmentRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_userAppointmentRequests_UserId",
                table: "userAppointmentRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_userAppointmentRequests_AspNetUsers_UserId",
                table: "userAppointmentRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
