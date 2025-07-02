
namespace MedicalAppointmentsSystem.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string SaveImage(IFormFile image, string folderPath, string userId)
        {
            //Get WWWRootPath
            string rootPath = _webHostEnvironment.WebRootPath;

            List<string> allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };
            var imageExtension = Path.GetExtension(image.FileName);

            if (!allowedExtensions.Contains(imageExtension))
                return "";

            string imageName = Guid.NewGuid().ToString() + imageExtension;

            string imagePath = Path.Combine(rootPath, folderPath);

            
            using(var fileStream = new FileStream(Path.Combine(imagePath, imageName), FileMode.Create))
            {
                image.CopyTo(fileStream);
            }


            
            return @"\Images\Doctors\" + userId + "\\" + imageName;
        }

        public string UpdateImage(IFormFile newImage, string oldImagePath, string folderPath, string userId)
        {
            List<string> allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };
            var imageExtension = Path.GetExtension(newImage.FileName);

            if (!allowedExtensions.Contains(imageExtension))
                return "";

            string rootPath = _webHostEnvironment.WebRootPath;

            string fullOldImagePath = rootPath + oldImagePath;

            File.Delete(fullOldImagePath);

            string newImageName = Guid.NewGuid().ToString() + imageExtension;

            string newImagePath = Path.Combine(rootPath, folderPath);


            using (var fileStream = new FileStream(Path.Combine(newImagePath, newImageName), FileMode.Create))
            {
                newImage.CopyTo(fileStream);
            }


            return @"\Images\Doctors\" + userId + "\\" + newImageName;
        }
    }
}
