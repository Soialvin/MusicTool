using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.DataAccess.Repositories.Helper;
using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.SingerRepositories
{
    public class SingerRepository : ISingerRepository
    {
        private readonly MusicToolDbContext _db;

        public SingerRepository(MusicToolDbContext db)
        {
            _db = db;
        }
        public async Task<Singer> AddAsync(Singer singer)
        {
            singer.NameNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(singer.Name);
            await _db.Singer.AddAsync(singer);
            await _db.SaveChangesAsync();
            return singer;
        }

        public async Task<Singer?> DeleteAsync(int id)
        {
            var existingSinger = await _db.Singer.FindAsync(id);
            if (existingSinger != null)
            {
                existingSinger.Status = false;
                await _db.SaveChangesAsync();
                return existingSinger;
            }
            return null;
        }

        public async Task<IEnumerable<Singer>> GetAllAsync()
        {
            return await _db.Singer.ToListAsync();
        }

        public async Task<Singer?> GetByIdAsync(int id)
        {
            return await _db.Singer.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IPagedList<Singer>?> GetPagingAsync(int page, int size)
        {
            var singers = await _db.Singer.Where(x => x.Status == true).ToPagedListAsync(page, size);
            if (singers.PageCount < page)
            {
                return null;
            }
            return singers;
        }

        public async Task<IPagedList<Singer>?> GetSingerByKeyPagingAsync(char key, int page, int size)
        {
            key = char.ToLower(key);
            var singers = await _db.Singer.Where(x => x.NameNoDiacritics.ToLower().StartsWith(key.ToString()) && x.Status == true).OrderBy(x => x.Name).ToPagedListAsync(page, size);
            if (singers.PageCount < page)
            {
                return null;
            }
            return singers;
        }

        public async Task<IPagedList<Singer>?> SearchAsync(string key, int page, int size)
        {
            if (key == null)
            {
                var singers = await _db.Singer.Where(x => x.Status == true).ToPagedListAsync(page, size);
                if (singers.PageCount < page)
                {
                    return null;
                }
                return singers;
            }
            else
            {
                string keyNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(key.ToLower());
                var singers = await _db.Singer.Where(x => x.NameNoDiacritics.ToLower().Contains(keyNoDiacritics) && x.Status == true).ToPagedListAsync(page, size);
                if (singers.PageCount < page)
                {
                    return null;
                }
                return singers;
            }
        }

        public async Task<Singer?> UpdateAsync(Singer singer)
        {
            var existingSinger = await _db.Singer.FindAsync(singer.Id);
            if (existingSinger != null)
            {
                existingSinger.Name = singer.Name;
                existingSinger.NameNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(existingSinger.Name);
                if (singer.PhotoName != null)
                {
                    existingSinger.PhotoName = singer.PhotoName;
                }
                await _db.SaveChangesAsync();
                return existingSinger;
            }
            return null;
        }
    }
}
