using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentsSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAppointmentResponsesTableAddingRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "UserAppointmentResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "UserAppointmentResponses");
        }
    }
}
