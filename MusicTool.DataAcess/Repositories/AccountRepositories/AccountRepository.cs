using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.Models.Domain;
using System.Text.RegularExpressions;
using X.PagedList;

namespace MusicTool.DataAccess.Repositories.AccountRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MusicToolDbContext _db;

        public AccountRepository(MusicToolDbContext db)
        {
            _db = db;
        }
        public async Task<Account?> AddAsync(Account account)
        {
            if (account.Name == null)
            {
                account.Name = account.UserName;
            }
            else
            {
                account.Name = account.Name.Trim();

                account.Name = Regex.Replace(account.Name, @"\s+", " ");
            }
            if (account.Type == true)
            {
                account.UserType = null;
            }
            else
            {
                account.UserType = "normal";
            }
            await _db.Account.AddAsync(account);
            await _db.SaveChangesAsync();
            return account;
        }

        public async Task<bool?> ExistingAsync(string data, string name)
        {
            if (name == "UserName")
            {
                return await _db.Account.AnyAsync(x => x.UserName == data);
            }
            if (name == "Email")
            {
                return await _db.Account.AnyAsync(x => x.Email == data);
            }
            return null;
        }

        public async Task<Account?> DeleteAsync(int id)
        {
            var existingAccount = await _db.Account.FindAsync(id);
            if (existingAccount != null)
            {
                existingAccount.Status = false;
                await _db.SaveChangesAsync();
                return existingAccount;
            }
            return null;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _db.Account.ToListAsync();
        }

        public Task<Account?> GetByIdAsync(int id)
        {
            return _db.Account.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Account?> UpdateAsync(Account account)
        {
            var existingAccount = await _db.Account.FindAsync(account.Id);
            if (existingAccount != null)
            {
                if (account.Name == null)
                {
                    account.Name = existingAccount.UserName;
                }
                else
                {
                    account.Name = account.Name.Trim();

                    account.Name = Regex.Replace(account.Name, @"\s+", " ");
                }
                if (account.Type == true)
                {
                    account.UserType = null;
                }
                else
                {
                    account.UserType = "normal";
                }
                if (account.PhotoName != null)
                {
                    existingAccount.PhotoName = account.PhotoName;
                }
                existingAccount.Name = account.Name;
                existingAccount.Email = account.Email;
                existingAccount.UserType = account.UserType;
                existingAccount.Type = account.Type;

                await _db.SaveChangesAsync();
                return existingAccount;
            }
            return null;
        }

        public async Task<IPagedList<Account>?> GetPagingAsync(int page, int size)
        {
            var accounts = await _db.Account.Where(x => x.Status == true).ToPagedListAsync(page, size);
            if (accounts.PageCount < page)
            {
                return null;
            }
            return accounts;
        }

        public async Task<IPagedList<Account>?> SearchAsync(string key, int page, int size)
        {
            if (key == null)
            {
                var accounts = await _db.Account.Where(x => x.Status == true).ToPagedListAsync(page, size);
                if (accounts.PageCount < page)
                {
                    return null;
                }
                return accounts;
            }
            else
            {
                var accounts = await _db.Account.Where(x => x.UserName.ToLower().Contains(key.ToLower()) && x.Status == true).ToPagedListAsync(page, size);
                if (accounts.PageCount < page)
                {
                    return null;
                }
                return accounts;
            }
        }

        public async Task<Account?> GetByUserNameAsync(Account account)
        {
            var existingAccount = await _db.Account.FirstOrDefaultAsync(x => x.UserName == account.UserName && x.Status == true);
            if (existingAccount == null)
            {
                return null;
            }
            if (existingAccount.Password == account.Password)
            {
                return existingAccount;
            }
            return null;
        }

        public async Task<Account?> UpdateUserAsync(Account account)
        {
            var existingAccount = await _db.Account.FindAsync(account.Id);
            if (existingAccount != null)
            {
                if (account.Name == null)
                {
                    account.Name = existingAccount.UserName;
                }
                else
                {
                    account.Name = account.Name.Trim();

                    account.Name = Regex.Replace(account.Name, @"\s+", " ");
                }
                if (account.PhotoName != null)
                {
                    existingAccount.PhotoName = account.PhotoName;
                }
                if (account.Password != null)
                {
                    existingAccount.Password = account.Password;
                }
                existingAccount.Name = account.Name;
                existingAccount.Email = account.Email;
                existingAccount.UserType = account.UserType;
                existingAccount.Type = account.Type;

                await _db.SaveChangesAsync();
                return existingAccount;
            }
            return null;
        }

        public async Task<Account?> UpdateUserTypeAsync(int accountId)
        {
            var existingAccount = await _db.Account.FindAsync(accountId);
            if (existingAccount != null)
            {
                if (existingAccount.UserType == "normal")
                {
                    existingAccount.UserType = "vip";
                    await _db.SaveChangesAsync();
                    return existingAccount;
                }
                return existingAccount;
            }
            return null;
        }
    }
}
