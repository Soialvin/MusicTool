using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.Models.Domain;

namespace MusicTool.DataAccess.Repositories.Song_SingerRepositories
{
    public class Song_SingerRepository : ISong_SingerRepository
    {
        private readonly MusicToolDbContext _db;

        public Song_SingerRepository(MusicToolDbContext db)
        {
            _db = db;
        }
        public async Task<Song_Singer> AddAsync(Song_Singer song_Singer)
        {
            await _db.Song_Singer.AddAsync(song_Singer);
            await _db.SaveChangesAsync();
            return song_Singer;
        }

        public async Task<Song_Singer?> DeleteAsync(int id)
        {
            var existingSong_Singer = await _db.Song_Singer.FindAsync(id);
            if (existingSong_Singer != null)
            {
                _db.Song_Singer.Remove(existingSong_Singer);
                await _db.SaveChangesAsync();
                return existingSong_Singer;
            }
            return null;
        }
        public async Task<List<int>?> GetByIdAsync(int songId)
        {
            return await _db.Song_Singer.Where(x => x.SongId == songId).Select(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<Song_Singer>> GetBySingerIdAsync(int singerId)
        {
            return await _db.Song_Singer.Where(x => x.SingerId == singerId).ToListAsync();
        }

        public async Task<IEnumerable<Song_Singer>> GetRelatedSongAsync(int id)
        {
            return await _db.Song_Singer.Where(x => x.SingerId == id).ToListAsync();
        }
    }
}
