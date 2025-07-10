using MedicalAppointmentsSystem.Models;

namespace MedicalAppointmentsSystem.Areas.User.Models
{
    public class UserAppointmentRequest
    {
        public int Id { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
        public string DoctorId { get; set; } = string.Empty;
        public ApplicationUser Doctor { get; set; }
        public string ClinicId { get; set; } = string.Empty;
        public Clinic Clinic { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string DocumentsUrl {  get; set; } = string.Empty;   
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
    }
}
