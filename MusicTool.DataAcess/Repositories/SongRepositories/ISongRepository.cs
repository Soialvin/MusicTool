using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.SongRepositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllAsync();
        Task<IPagedList<Song>?> GetPagingAsync(int page, int size);
        Task<Song?> GetByIdAsync(int id);
        Task<Song> AddAsync(Song song);
        Task<Song?> UpdateAsync(Song song);
        Task<Song?> DeleteAsync(int id);
        Task<IPagedList<Song>?> SearchAsync(string key, int page, int size);
        Task<IEnumerable<Song>?> GetTOPAsync(string type, int quantity);
        Task<Song?> UpdateNumberOfListensAsync(Song song);
        Task<Song?> UpdateNumberOfDownloadsAsync(Song song);
        Task<IEnumerable<Song>?> AutoSearchAsync(string key);
    }
}
