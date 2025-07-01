using System.ComponentModel.DataAnnotations;

namespace MedicalAppointmentsSystem.Views.ViewModels
{
    public class DoctorInformationVM
    {
        public string DoctorName { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;


        public string Specialization { get; set; } = string.Empty;


        public int YearsExperience { get; set; }

        public string MedicalSchool { get; set; } = string.Empty;


        public int? GraduationYear { get; set; }


        public string HospitalAffiliation { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;


        public string Bio { get; set; } = string.Empty;


        public string EmergencyPhone { get; set; } = string.Empty;
        public string LicenseImagePath { get; set; } = string.Empty;

        public string IdProofPath { get; set; } = string.Empty;

        public string PhotoPath { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}
