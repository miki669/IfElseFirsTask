using System.ComponentModel.DataAnnotations;

namespace IfElseFirsTask.Context.Model;

public class Account
{
    [Key]
    public int Id { get; set; }
}