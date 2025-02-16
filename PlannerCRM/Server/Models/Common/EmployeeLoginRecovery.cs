namespace PlannerCRM.Server.Models.Common;

public class EmployeeLoginRecovery
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmNewPassword { get; set; }

    public string Phone { get; set; }
}
