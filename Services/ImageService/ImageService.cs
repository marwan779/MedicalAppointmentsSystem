
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
    }
}
