using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.DataAccess.Repositories.Helper;
using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.SongRepositories
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicToolDbContext _db;

        public SongRepository(MusicToolDbContext db)
        {
            _db = db;
        }
        public async Task<Song> AddAsync(Song song)
        {
            song.NameNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(song.Name);
            await _db.Song.AddAsync(song);
            await _db.SaveChangesAsync();
            return song;
        }

        public async Task<IEnumerable<Song>?> AutoSearchAsync(string key)
        {
            string keyNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(key.ToLower());
            return await _db.Song.Where(x => x.NameNoDiacritics.ToLower().Contains(keyNoDiacritics) && x.Status == true).ToListAsync();
        }

        public async Task<Song?> DeleteAsync(int id)
        {
            var existingSong = await _db.Song.FindAsync(id);
            if (existingSong != null)
            {
                existingSong.Status = false;
                await _db.SaveChangesAsync();
                return existingSong;
            }
            return null;
        }

        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _db.Song.ToListAsync();
        }

        public Task<Song?> GetByIdAsync(int id)
        {
            return _db.Song.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IPagedList<Song>?> GetPagingAsync(int page, int size)
        {
            var songs = await _db.Song.Where(x => x.Status == true).ToPagedListAsync(page, size);
            if (songs.PageCount < page)
            {
                return null;
            }
            return songs;
        }

        public async Task<IEnumerable<Song>?> GetTOPAsync(string type, int quantity)
        {
            if (quantity == 0 || type == null)
            {
                return null;
            }
            if (type == "NewSong")
            {
                return await _db.Song.Where(x => x.Status == true).OrderByDescending(x => x.LoadingDate).Take(quantity).ToListAsync();
            }
            if (type == "SongList")
            {
                return await _db.Song.Where(x => x.Status == true).Take(quantity).ToListAsync();
            }
            if (type == "ListeningRantings")
            {
                return await _db.Song.Where(x => x.Status == true).OrderByDescending(x => x.NumberOfListens).Take(quantity).ToListAsync();
            }
            if (type == "DownloadRantings")
            {
                return await _db.Song.Where(x => x.Status == true).OrderByDescending(x => x.NumberOfDownloads).Take(quantity).ToListAsync();
            }
            return null;
        }

        public async Task<IPagedList<Song>?> SearchAsync(string key, int page, int size)
        {
            if (key != null)
            {
                string keyNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(key.ToLower());
                var songs = await _db.Song.Where(x => x.NameNoDiacritics.ToLower().Contains(keyNoDiacritics) && x.Status == true).ToPagedListAsync(page, size);
                if (songs.PageCount < page)
                {
                    return null;
                }
                return songs;
            }
            else
            {
                var songs = await _db.Song.Where(x => x.Status == true).ToPagedListAsync(page, size);
                if (songs.PageCount < page)
                {
                    return null;
                }
                return songs;
            }
        }

        public async Task<Song?> UpdateAsync(Song song)
        {
            var existingSong = await _db.Song.FindAsync(song.Id);
            if (existingSong != null)
            {
                existingSong.Name = song.Name;
                existingSong.NameNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(existingSong.Name);
                existingSong.Lyrics = song.Lyrics;
                if (song.PhotoName != null)
                {
                    existingSong.PhotoName = song.PhotoName;
                }
                if (song.SongFile != null)
                {
                    existingSong.SongFile = song.SongFile;
                }

                await _db.SaveChangesAsync();
                return existingSong;
            }
            return null;
        }

        public async Task<Song?> UpdateNumberOfDownloadsAsync(Song song)
        {
            var existingSong = await _db.Song.FindAsync(song.Id);
            if (existingSong != null)
            {
                existingSong.NumberOfDownloads = existingSong.NumberOfDownloads + 1;
                await _db.SaveChangesAsync();
                return existingSong;
            }
            return null;
        }

        public async Task<Song?> UpdateNumberOfListensAsync(Song song)
        {
            var existingSong = await _db.Song.FindAsync(song.Id);
            if (existingSong != null)
            {
                existingSong.NumberOfListens = existingSong.NumberOfListens + 1;
                await _db.SaveChangesAsync();
                return existingSong;
            }
            return null;
        }
    }
}
