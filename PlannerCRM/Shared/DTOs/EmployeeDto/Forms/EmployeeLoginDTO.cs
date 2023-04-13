using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeLoginDTO
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Role { get; set; }
}