using MusicTool.Models.Domain;

namespace MusicTool.DataAccess.Repositories.Song_SingerRepositories
{
    public interface ISong_SingerRepository
    {
        Task<Song_Singer> AddAsync(Song_Singer song_Singer);
        Task<List<int>?> GetByIdAsync(int songId);
        Task<IEnumerable<Song_Singer>> GetBySingerIdAsync(int singerId);
        Task<Song_Singer?> DeleteAsync(int id);
        Task<IEnumerable<Song_Singer>> GetRelatedSongAsync(int id);
    }
}
