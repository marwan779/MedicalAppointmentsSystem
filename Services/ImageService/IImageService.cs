namespace MedicalAppointmentsSystem.Services.ImageService
{
    public interface IImageService
    {
        string SaveImage(IFormFile image, string folderPath, string userId);
        string UpdateImage(IFormFile newImage, string oldImagePath, string folderPath, string userId);
    }
}
