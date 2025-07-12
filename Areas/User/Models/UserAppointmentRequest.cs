using MedicalAppointmentsSystem.Models;

namespace MedicalAppointmentsSystem.Areas.User.Models
{
    public class UserAppointmentRequest
    {
        public int Id { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public int ClinicId { get; set; } 
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateOnly PreferredDate { get; set; }
        public string? DocumentsUrl {  get; set; } = string.Empty;   
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
        public string ComplainsAbout { get; set; } = string.Empty;
        public string? ChronicDiseases {  get; set; } = string.Empty;    
    }
}
