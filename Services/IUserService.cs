using IfElseFirsTask.Context.Model;

namespace IfElseFirsTask.Services;

public interface IUserService
{
    Task<Account> RegisterAccount(Account user);
    Task<Account?> GetAccount(int id);
    Task<List<Account>?> SearchAccounts(string firstName, string lastName, string email, int from = 0, int size = 10);
    Task<Account?> UpdateAccount(int accountid,Account account);
    Task<Account?> GetAccountByEmailAsync(string email);
    bool AccountValidity(int id,string password);
    Task<bool> DeleteAccount(int id);
}