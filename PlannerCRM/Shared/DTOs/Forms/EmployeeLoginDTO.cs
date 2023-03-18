using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.DTOs.Abstract;

namespace PlannerCRM.Shared.DTOs;

public class EmployeeLoginDTO : EmployeeDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}