
namespace MedicalAppointmentsSystem.Services.ImageService
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string SaveFile(IFormFile file, string folderPath, string fileRootPath)
        {
            //Get WWWRootPath
            string rootPath = _webHostEnvironment.WebRootPath;

            //List<string> allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };
            //var imageExtension = Path.GetExtension(image.FileName);

            //if (!allowedExtensions.Contains(imageExtension))
            //    return "";

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            string filePath = Path.Combine(rootPath, folderPath);

            
            using(var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }


            
            return fileRootPath + fileName;
        }

        public string UpdateFile(IFormFile newFile, string oldFilePath, string folderPath, string FileRootPath)
        {
            //List<string> allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };
            //var imageExtension = Path.GetExtension(newImage.FileName);

            //if (!allowedExtensions.Contains(imageExtension))
            //    return "";

            string rootPath = _webHostEnvironment.WebRootPath;

            string fullOldFilePath = rootPath + oldFilePath;

            File.Delete(fullOldFilePath);

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(newFile.FileName);

            string newFilePath = Path.Combine(rootPath, folderPath);


            using (var fileStream = new FileStream(Path.Combine(newFilePath, newFileName), FileMode.Create))
            {
                newFile.CopyTo(fileStream);
            }


            return FileRootPath + newFileName;
        }
    }
}
