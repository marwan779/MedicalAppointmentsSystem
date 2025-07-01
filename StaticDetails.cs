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
    }
}
