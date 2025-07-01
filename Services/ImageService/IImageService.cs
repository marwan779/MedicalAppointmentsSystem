namespace MedicalAppointmentsSystem.Services.ImageService
{
    public interface IImageService
    {
        string SaveImage(IFormFile image, string folderPath, string userId);
    }
}
