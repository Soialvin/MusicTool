using MusicTool.Models.Domain;
using MusicTool.Models.DTO;

namespace MusicTool.DataAccess.Repositories.VipRepositories
{
    public interface IVipRepository
    {
        Task<VIP> AddAsync(VIP vip);
        Task<List<Revenue>?> GetRevenuesAsync(int year);
        Task<List<int>> GetAllYearAsync();
    }
}
