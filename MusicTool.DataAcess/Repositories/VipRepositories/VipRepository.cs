using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.Models.Domain;
using MusicTool.Models.DTO;

namespace MusicTool.DataAccess.Repositories.VipRepositories
{
    public class VipRepository : IVipRepository
    {
        private readonly MusicToolDbContext _db;

        public VipRepository(MusicToolDbContext db)
        {
            _db = db;
        }
        public async Task<VIP> AddAsync(VIP vip)
        {
            await _db.VIP.AddAsync(vip);
            await _db.SaveChangesAsync();
            return vip;
        }

        public async Task<List<int>> GetAllYearAsync()
        {
            return await _db.VIP.Select(x => x.VIPRegistrationDate.Value.Year).Distinct().OrderByDescending(year => year).ToListAsync();
        }

        public async Task<List<Revenue>?> GetRevenuesAsync(int year)
        {
            var revenues = await _db.Set<Revenue>()
                            .FromSqlInterpolated($"EXEC ThongKeDoanhThu @Year = {year}")
                            .ToListAsync();
            if (revenues.Count > 0)
            {
                return revenues;
            }
            return null;
        }
    }
}
