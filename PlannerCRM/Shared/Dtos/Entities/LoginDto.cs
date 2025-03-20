namespace PlannerCRM.Shared.Dtos.Entities;

public class LoginDto
{
    [Required(ErrorMessage = "Email/Username field is required")]
    public string emailOrUsername { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string password { get; set; }

    public bool rememberMe { get; set; }
}


public partial class ApiUrl
{
    public const string ACCOUNT_CONTROLLER = "api/account";

    public const string ACCOUNT_LOGIN = "login";
}