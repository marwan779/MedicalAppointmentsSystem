namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class ComplaintVM
    {
        public int Id { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string ComplaintTitle { get; set; } = string.Empty;  
        public string ComplaintStatus{ get; set; } = StaticDetails.ComplaintUnderReview;
        public string? DocumentUrl { get; set; } = string.Empty;
        public DateTime FiledAt { get; set; } 
    }
}
