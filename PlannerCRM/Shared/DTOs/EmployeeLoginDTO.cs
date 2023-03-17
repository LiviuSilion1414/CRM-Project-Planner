using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs;

public class EmployeeLoginDTO 
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}