using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vizibil_api.Entities;

public class Contact
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    public string CountryCode { get; set; } = null!;
    
    [Phone]
    public string Phone { get; set; } = null!;
    
    [Url]
    public string? Website { get; set; }
    
    public string? Budget { get; set; }
    public string? Country { get; set; }
    public string Service { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Date { get; set; }
}