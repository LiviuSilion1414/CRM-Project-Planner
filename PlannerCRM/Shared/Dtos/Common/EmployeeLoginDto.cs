namespace PlannerCRM.Shared.Dtos.Common;

public class EmployeeLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Length(8, 16)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
