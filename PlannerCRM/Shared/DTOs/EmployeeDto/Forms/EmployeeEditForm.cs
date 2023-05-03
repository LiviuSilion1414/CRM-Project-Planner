using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeEditForm
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

    public decimal HourPay { get; set; }

    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}