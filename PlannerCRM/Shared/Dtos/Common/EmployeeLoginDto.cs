namespace PlannerCRM.Shared.Dtos.Common;

public class EmployeeLoginDto
{
    [Required]
    public string EmailOrUsername { get; set; }

    [Required]
    [Length(8, 16)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
