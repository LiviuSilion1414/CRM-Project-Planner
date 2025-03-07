namespace PlannerCRM.Shared.Dtos.Common;

public class LoginDto
{
    [Required]
    public string EmailOrUsername { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
