namespace PlannerCRM.Shared.Dtos.Common;

public class EmployeeLoginRecoveryDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [Phone]
    public string Phone { get; set; }

    public string Password { get; set; }
    
    public string NewPassword { get; set; }
    
    public string ConfirmNewPassword { get; set; }
}
