namespace PlannerCRM.Shared.Dtos.Entities;

public class LoginDto
{
    [Required]
    public string EmailOrUsername { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}


public partial class ApiUrl
{
    public const string ACCOUNT_CONTROLLER = "api/account";

    public const string ACCOUNT_LOGIN = "login";
}