namespace Moto.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveFile(IFormFile file, string prefix="");
        public Task<bool> DeleteFile(string fileName, string prefix = "");
    }
}
