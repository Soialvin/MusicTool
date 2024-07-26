using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MusicTool.Models.DTO;

namespace MusicTool.DataAccess.Repositories.FileUploadRepositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly IHostingEnvironment _ht;

        public FileUploadRepository(IHostingEnvironment ht)
        {
            _ht = ht;
        }
        public async Task<FileUpdate> UploadFileAsync(IFormFile file, string location)
        {
            var oneName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(_ht.ContentRootPath, location, oneName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            var fileUpdate = new FileUpdate()
            {
                FileName = oneName,
                FilePath = filePath,
            };

            return fileUpdate;
        }
    }
}
