using System.ComponentModel.DataAnnotations;

namespace IfElseFirsTask.Context.Model;

public class AccountToken
{
    [Key]
    public long Id { get; set; }
    public string Token { get; set; }
    public Account Account { get; set; }
    public DateTime TokenExpirationTime { get; set; }

}