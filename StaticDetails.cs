namespace MedicalAppointmentsSystem
{
    public static class StaticDetails
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";
        public const string DoctorRole = "Doctor";

        public enum Types
        {
            Doctor,
            User,
            Admin
        };

        public static List<string> MedicalSpecializations = new List<string>
        {
        "Cardiology",
        "Dermatology",
        "Neurology",
        "Pediatrics",
        "Orthopedics",
        "General Surgery",
        "Internal Medicine",
        "Obstetrics and Gynecology (OB/GYN)",
        "Psychiatry",
        "Radiology",
        "Anesthesiology",
        "Ophthalmology",
        "Otolaryngology (ENT)",
        "Urology",
        "Endocrinology",
        "Oncology",
        "Gastroenterology",
        "Nephrology",
        "Rheumatology"
        };

        public const string RequestUnderReview = "Under Review";
        public const string AppointmentScheduled = "Appointment Scheduled";
        public const string RequestCanceled = "Appointment Canceled";

        public static List<string> allowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
    }
}
