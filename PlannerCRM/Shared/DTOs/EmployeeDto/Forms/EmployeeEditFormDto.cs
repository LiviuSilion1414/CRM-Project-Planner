using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeEditFormDto
{
    public int Id { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string FirstName { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string LastName { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string Password { get; set; } = default;
        
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string Email { get; set; }
    
    [Editable(allowEdit: false, AllowInitialValue = true)]
    public string OldEmail { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime StartDate { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime BirthDay { get; set; } 

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string NumericCode { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    [EnumDataType(typeof(Roles))]
    public Roles Role { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public decimal? CurrentHourlyRate { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime? StartDateHourlyRate { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime? FinishDateHourlyRate { get; set; }

    [Editable(allowEdit: false, AllowInitialValue = true)]
    public bool IsDeleted { get; set; }

    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
}