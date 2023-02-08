using IfElseFirsTask.Context;
using IfElseFirsTask.Context.Model;

using IfElseFirsTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IfElseFirsTask.Controllers;


[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly UserService _userService;
    private readonly PrimaryDataBaseContext _dbContext;
    public AccountsController(ILogger<AccountsController> logger,UserService userService,PrimaryDataBaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userService = userService;

    }

    [HttpPost("/registration")]
    public async Task<ActionResult<Account>> RegisterUser(Account account)
    {
        var user = await _userService.RegisterAccount(account);
        
        return new Account()
        {
            Email = user.Email,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
    
    [HttpGet("/account/{accountId:int}")]
    public async Task<IActionResult> GetAccount(int accountId)
    {
        if (accountId <= 0)
        {
            return BadRequest("accountId <= 0");
        }
        if (accountId == null)
        {
            return BadRequest("accountId = null,");
        }
        var user = await _userService.GetAccount(accountId);
        if (user == null)
        {
            return BadRequest("No account found");
        }
        return Ok(new Account()
        {
            Email = user.Email,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }
    [HttpGet("/accounts/search")]
    public async Task<ActionResult<IEnumerable<Account>>> SearchAccounts(string? firstName, string? lastName, string? email, int from = 0, int size = 10)
    {
        if (from < 0)
        {
            return BadRequest("The 'from' parameter must be greater than or equal to 0");
        }

        if (size <= 0)
        {
            return BadRequest("The 'size' parameter must be greater than 0");
        }
        var users =  await _userService.SearchAccounts(firstName, lastName, email, from, size);
        if (users.Count <=0)
        {
            return Unauthorized("Invalid authorization data");
        }
        var result = users.Select(user => new Account
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        });

        return Ok(result);
    }
    
    [HttpPut("/accounts")]
    public async  Task<IActionResult> UpdateAccount(int accountid, [FromBody] Account account)
    {
        

        if (string.IsNullOrEmpty(account.FirstName) || account.FirstName.IndexOf(' ')>-1 &&
            string.IsNullOrEmpty(account.LastName) || account.LastName.IndexOf(' ') > -1 &&
            string.IsNullOrEmpty(account.Email) || account.Email.IndexOf(' ') > -1 &&
            string.IsNullOrEmpty(account.Password) || account.Password.IndexOf(' ') > -1 &&
            accountid == null || accountid <= 0
            )
        {
            return BadRequest(
                "accountId = null," +
                "\naccountId <= 0," +
                "\nfirstName = null," +
                "\nfirstName = \"\" или состоит из пробелов," +
                "\nlastName = null," +
                "\nlastName = \"\" или состоит из пробелов," +
                "\nemail = null, email = \"\" или состоит из пробелов," +
                "\nemail аккаунта не валидный," +
                "\npassword = null, password = \"\" или состоит из пробелов");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingAccount = await _userService.GetAccount(accountid);
        if (existingAccount == null)
        {
            return StatusCode(401,"Аккаунт не найден");
        }
        var existingEmailAccount = await _userService.GetAccountByEmailAsync(account.Email);
        if (existingEmailAccount != null && existingEmailAccount.Id != accountid)
        {
            return Conflict("Аккаунт с таким 'email' уже существует");
        }
        if(!_userService.AccountValidity(accountid, account.Password))
            return StatusCode(403,"Обновление не своего аккаунта \nАккаунт не найден");
        
        var updatedAccount = await _userService.UpdateAccount(accountid,account);
        return Ok(new Account
        {
            Id = updatedAccount.Id,
            FirstName = updatedAccount.FirstName,
            LastName = updatedAccount.LastName,
            Email = updatedAccount.Email
        });

    }
   
    
    [HttpDelete("/accounts/")]
     public async  Task<IActionResult> DeleteAccount(int accountid)
    {
        

        var account = await _dbContext.Account.FindAsync(accountid);
        if (account == null)
        {
            return NotFound();
        }

        _dbContext.Account.Remove(account);
        await _dbContext.SaveChangesAsync();

        return NoContent();

    }
    

}