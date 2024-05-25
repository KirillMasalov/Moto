using Moto.Services.Interfaces;

namespace Moto.Services
{
    public class FileService: IFileService
    {
        private IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment env) 
        {
            environment = env;
        }

        public async Task<string> SaveFile(IFormFile file, string prefix = "") 
        {
            var uploads = Path.Combine(environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);
            
            uploads = Path.Join(uploads, prefix);

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            if (file.Length > 0)
            {
                var fileParts = file.FileName.Split('.');
                var fileName = $"{fileParts[0]}_{DateTime.Now.Ticks.ToString()}.{fileParts[1]}";
                var filePath = Path.Combine(uploads, fileName);
                Console.WriteLine(fileName);
                Console.WriteLine(filePath);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return fileName;
            }
            return null;
        }

        public async Task<bool> DeleteFile(string fileName, string prefix = "")
        {
            return true;
        }
    }
}
