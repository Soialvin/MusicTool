using MusicTool.Models.Domain;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<IPagedList<Account>?> GetPagingAsync(int page, int size);
        Task<Account?> GetByIdAsync(int id);
        Task<Account?> GetByUserNameAsync(Account account);
        Task<Account?> AddAsync(Account account);
        Task<Account?> UpdateAsync(Account account);
        Task<Account?> UpdateUserAsync(Account account);
        Task<Account?> UpdateUserTypeAsync(int accountId);
        Task<Account?> DeleteAsync(int id);
        Task<IPagedList<Account>?> SearchAsync(string key, int page, int size);
        Task<bool?> ExistingAsync(string data, string name);
    }
}
