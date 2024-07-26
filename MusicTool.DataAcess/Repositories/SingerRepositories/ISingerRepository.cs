using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.SingerRepositories
{
    public interface ISingerRepository
    {
        Task<IEnumerable<Singer>> GetAllAsync();
        Task<IPagedList<Singer>?> GetPagingAsync(int page, int size);
        Task<IPagedList<Singer>?> GetSingerByKeyPagingAsync(char key, int page, int size);
        Task<Singer?> GetByIdAsync(int id);
        Task<Singer> AddAsync(Singer singer);
        Task<Singer?> UpdateAsync(Singer singer);
        Task<Singer?> DeleteAsync(int id);
        Task<IPagedList<Singer>?> SearchAsync(string key, int page, int size);
    }
}
