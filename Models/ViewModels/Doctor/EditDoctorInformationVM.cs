using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Models.ViewModels.Doctor
{
    public class EditDoctorInformationVM
    {
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Medical license number is required")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; } = string.Empty;
        [ValidateNever]
        public IEnumerable<SelectListItem> SpecializationList { get; set; }

        [Required(ErrorMessage = "Years of experience is required")]
        public int YearsExperience { get; set; }

        [Required(ErrorMessage = "Medical school is required")]
        public string MedicalSchool { get; set; } = string.Empty;

        [Required(ErrorMessage = "Graduation year is required")]
        public int? GraduationYear { get; set; }

        [Required(ErrorMessage = "Hospital affiliation is required")]
        public string HospitalAffiliation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bio is required")]
        public string Bio { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency phone is required")]
        public string EmergencyPhone { get; set; } = string.Empty;

        [ValidateNever]
        public IFormFile LicenseImage { get; set; }
        [ValidateNever]
        public IFormFile IdProof { get; set; }
        [ValidateNever]
        public IFormFile Photo { get; set; }
        [ValidateNever]
        public string LicenseImagePath { get; set; } = string.Empty;
        [ValidateNever]
        public string IdProofPath { get; set; } = string.Empty;
        [ValidateNever]
        public string PhotoPath { get; set; } = string.Empty;

    }
}
