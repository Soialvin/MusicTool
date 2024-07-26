using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.CategoryRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IPagedList<Category>?> GetPagingAsync(int page, int size);
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(int id);
        Task<IPagedList<Category>?> SearchAsync(string key, int page, int size);
    }
}
