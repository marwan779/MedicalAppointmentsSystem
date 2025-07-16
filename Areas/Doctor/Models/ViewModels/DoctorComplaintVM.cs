namespace MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels
{
    public class DoctorComplaintVM
    {
        public int Id { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string ComplaintTitle { get; set; } = string.Empty;
        public string ComplaintStatus { get; set; } = StaticDetails.ComplaintUnderReview;
        public string? DocumentUrl { get; set; } = string.Empty;
        public DateTime FiledAt { get; set; }
        public bool DoctorResponded { get; set; } = false;
    }
}
