using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeLoginDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = """Campo "Email" richiesto""")]
    public string Email { get; set; }

    [Required(ErrorMessage = """Campo "Password" richiesto""")]
    public string Password { get; set; }

    public string Role { get; set; }
}