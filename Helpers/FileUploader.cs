namespace Luno_platform.Helpers
{
    public class FileUploader
    {
        public static string UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/imgs");
            // Ensure the folder exists
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folder, uniqueFileName);
            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return $"/assets/imgs/{uniqueFileName}";
        }
    }
}
