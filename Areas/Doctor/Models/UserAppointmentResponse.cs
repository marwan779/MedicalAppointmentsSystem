namespace MedicalAppointmentsSystem.Areas.Doctor.Models
{
    public class UserAppointmentResponse
    {
        public int Id { get; set; }
        public string DoctorId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public int RequestId { get; set; }
        public TimeSpan AppointmentStartTime { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string AppointmentLocation { get; set; } = string.Empty;
        public string AppointmentDay { get; set; } = string.Empty;
    }
}
