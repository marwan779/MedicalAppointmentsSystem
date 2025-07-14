using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentsSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAppointmentResponsesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAppointmentResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicId = table.Column<int>(type: "int", nullable: false),
                    AppointmentStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AppointmentLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppointmentResponses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppointmentResponses");
        }
    }
}
