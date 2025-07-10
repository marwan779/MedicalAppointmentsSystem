namespace MedicalAppointmentsSystem.Services.ImageService
{
    public interface IFileService
    {
        string SaveFile(IFormFile file, string folderPath, string fileRootPath);
        string UpdateFile(IFormFile newFile, string oldFilePath, string folderPath, string FileRootPath);
    }
}
