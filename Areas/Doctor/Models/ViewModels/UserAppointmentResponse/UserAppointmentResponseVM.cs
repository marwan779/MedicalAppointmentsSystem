namespace MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.UserAppointmentResponse
{
    public class UserAppointmentResponseVM
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateOnly PreferredDate { get; set; }
        public string? DocumentsUrl { get; set; } = string.Empty;
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
        public string ComplainsAbout { get; set; } = string.Empty;
        public string? ChronicDiseases { get; set; } = string.Empty;
    }
}
