namespace PlannerCRM.Shared.Dtos.Entities;

public class LoginRecoveryDto
{
    public Guid id { get; set; }

    public string name { get; set; }

    [Required]
    [EmailAddress]
    public string email { get; set; }

    public string phone { get; set; }

    public string password { get; set; }

    public string newPassword { get; set; }

    public string confirmNewPassword { get; set; }
}
