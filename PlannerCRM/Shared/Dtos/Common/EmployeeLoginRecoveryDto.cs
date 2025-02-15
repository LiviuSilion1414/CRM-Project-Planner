namespace PlannerCRM.Shared.Dtos.Common;

public class EmployeeLoginRecoveryDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Phone { get; set; }

    public string Password { get; set; }
    
    public string NewPassword { get; set; }
    
    public string ConfirmNewPassword { get; set; }
}
