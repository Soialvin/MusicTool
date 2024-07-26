using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.DataAccess.Repositories.Helper;
using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.CategoryRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MusicToolDbContext _db;

        public CategoryRepository(MusicToolDbContext db)
        {
            _db = db;
        }
        public async Task<Category> AddAsync(Category category)
        {
            category.NameNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(category.Name);
            await _db.Category.AddAsync(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var existingCategory = await _db.Category.FindAsync(id);
            if (existingCategory != null)
            {
                existingCategory.Status = false;
                await _db.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Category.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Category.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IPagedList<Category>?> GetPagingAsync(int page, int size)
        {
            var categorys = await _db.Category.Where(x => x.Status == true).ToPagedListAsync(page, size);
            if (categorys.PageCount < page)
            {
                return null;
            }
            return categorys;
        }

        public async Task<IPagedList<Category>?> SearchAsync(string key, int page, int size)
        {
            if (key == null)
            {
                var categorys = await _db.Category.Where(x => x.Status == true).ToPagedListAsync(page, size);
                if (categorys.PageCount < page)
                {
                    return null;
                }
                return categorys;
            }
            else
            {
                string keyNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(key.ToLower());
                var categorys = await _db.Category.Where(x => x.NameNoDiacritics.ToLower().Contains(keyNoDiacritics) && x.Status == true).ToPagedListAsync(page, size);
                if (categorys.PageCount < page)
                {
                    return null;
                }
                return categorys;
            }
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _db.Category.FindAsync(category.Id);
            if (existingCategory != null)
            {
                if (category.PhotoName != null)
                {
                    existingCategory.PhotoName = category.PhotoName;
                }
                existingCategory.Name = category.Name;
                existingCategory.NameNoDiacritics = VietnameseHelper.RemoveVietnameseDiacritics(category.Name);
                await _db.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }
    }
}
