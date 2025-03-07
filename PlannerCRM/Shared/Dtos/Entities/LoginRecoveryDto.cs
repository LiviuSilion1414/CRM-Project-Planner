namespace PlannerCRM.Shared.Dtos.Entities;

public class LoginRecoveryDto
{
    public Guid Guid { get; set; }

    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Phone { get; set; }

    public string Password { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmNewPassword { get; set; }
}
