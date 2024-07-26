using Microsoft.AspNetCore.Http;
using MusicTool.Models.DTO;

namespace MusicTool.DataAccess.Repositories.FileUploadRepositories
{
    public interface IFileUploadRepository
    {
        Task<FileUpdate> UploadFileAsync(IFormFile file, string location);
    }
}
