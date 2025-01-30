namespace PlannerCRM.Shared.Dtos.Common;

public class EmployeeLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [PasswordValidator]
    [Length(8, 16)]
    public string Password { get; set; }
}
