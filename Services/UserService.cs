using IfElseFirsTask.Context;
using IfElseFirsTask.Context.Model;
using Microsoft.EntityFrameworkCore;

namespace IfElseFirsTask.Services;

public class UserService : IUserService
{
    private readonly PrimaryDataBaseContext _dbContext;

    public UserService(PrimaryDataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<Account> RegisterAccount(Account user)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetAccount(int id)
    {
        return await _dbContext.Account.FirstOrDefaultAsync(a => a.Id == id) ?? null;
    }

    public Task<List<Account>?> SearchAccounts(string firstName, string lastName, string email, int from = 0,
        int size = 10)
    {
        return Task.FromResult(_dbContext.Account
            .Skip(from)
            .Take(size)
            .Where(p =>
                (string.IsNullOrEmpty(lastName) || p.LastName.Contains(lastName.ToLower())) &&
                (string.IsNullOrEmpty(firstName) || p.FirstName.Contains(firstName.ToLower())) &&
                (string.IsNullOrEmpty(email) || p.Email.Contains(email.ToLower())))
            .OrderBy(x => x.Id)
            .Select(user => new Account()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            })
            .ToList());
    }

    public async Task<Account> UpdateAccount(int accountid,Account account)
    {
        var existingAccount = await GetAccount(accountid);
        existingAccount.FirstName = account.FirstName;
        existingAccount.LastName = account.LastName;
        existingAccount.Email = account.Email;
        await _dbContext.SaveChangesAsync();
        return existingAccount;
    }

    public async Task<Account?> GetAccountByEmailAsync(string email)
    {
        return await _dbContext.Account.FirstOrDefaultAsync(a => a.Email == email);
    }

    public bool AccountValidity(int id, string password)
    {
        return _dbContext.Account.Any(p => p.Id == id && p.Password == password);
    }

    public Task<bool> DeleteAccount(int id)
    {
        throw new NotImplementedException();
    }
}