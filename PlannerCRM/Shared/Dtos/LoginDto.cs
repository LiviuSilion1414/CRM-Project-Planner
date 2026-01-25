namespace PlannerCRM.Shared.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Email/Username field is required")]
    public string? username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? password { get; set; }

    public bool rememberMe { get; set; }
}