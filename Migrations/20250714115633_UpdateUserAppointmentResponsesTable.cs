using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentsSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAppointmentResponsesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentDay",
                table: "UserAppointmentResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDay",
                table: "UserAppointmentResponses");
        }
    }
}
