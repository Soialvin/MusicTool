using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.Song_CategoryRepositories
{
    public interface ISong_CategoryRepository
    {
        Task<IPagedList<Song>?> GetSongByCategoryIdAsync(int id, int page, int size);
        Task<Song_Category> AddAsync(Song_Category song_Category);
        Task<Song_Category?> DeleteAsync(int id);
        Task<List<int>?> GetByIdAsync(int songId);
    }
}
