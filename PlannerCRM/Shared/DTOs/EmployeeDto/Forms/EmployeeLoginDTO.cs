using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeLoginDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Role { get; set; }
}