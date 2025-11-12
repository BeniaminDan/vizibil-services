namespace vizibil_api.Models;

public class ContactModel
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Website { get; set; }
    public string? Budget { get; set; }
    public string? Country { get; set; }
    public string Service { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime? Date { get; set; }
}