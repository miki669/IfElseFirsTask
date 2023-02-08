using System.ComponentModel.DataAnnotations;

namespace IfElseFirsTask.Context.Model;

public class AnimalVisitedLocation
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}