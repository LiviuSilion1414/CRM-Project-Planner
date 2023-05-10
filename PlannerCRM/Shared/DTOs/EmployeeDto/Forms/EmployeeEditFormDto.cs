using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeEditFormDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime BirthDay { get; set; }
    
    public string NumericCode { get; set; }

    public Roles Role { get; set; }

    public float HourlyRate { get; set; }

    public DateTime? StartDateHourlyRate { get; set; }

    public DateTime? FinishDateHourlyRate { get; set; }

    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }

    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}