using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.Song_CategoryRepositories
{
    public class Song_CategoryRepository : ISong_CategoryRepository
    {
        private readonly MusicToolDbContext _db;

        public Song_CategoryRepository(MusicToolDbContext db)
        {
            _db = db;
        }

        public async Task<Song_Category> AddAsync(Song_Category song_Category)
        {
            await _db.Song_Category.AddAsync(song_Category);
            await _db.SaveChangesAsync();
            return song_Category;
        }

        public async Task<Song_Category?> DeleteAsync(int id)
        {
            var existingSong_Category = await _db.Song_Category.FindAsync(id);
            if (existingSong_Category != null)
            {
                _db.Song_Category.Remove(existingSong_Category);
                await _db.SaveChangesAsync();
                return existingSong_Category;
            }
            return null;
        }

        public async Task<List<int>?> GetByIdAsync(int songId)
        {
            return await _db.Song_Category.Where(x => x.SongId == songId).Select(x => x.Id).ToListAsync();
        }

        public async Task<IPagedList<Song>?> GetSongByCategoryIdAsync(int id, int page, int size)
        {
            var songCategoryList = await _db.Song_Category.Where(x => x.CategoryId == id).ToListAsync();
            if (songCategoryList.Count() == 0)
            {
                return null;
            }
            var songs = songCategoryList.Select(x => x.Song).ToPagedList(page, size);
            if (songs.PageCount < page || songs.Count() == 0)
            {
                return null;
            }
            return songs;
        }
    }
}
