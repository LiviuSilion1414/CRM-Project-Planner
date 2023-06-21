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

    //[StrongPasswordValidator(PASS_MIN_LENGTH, PASS_MAX_LENGTH,
    //    ErrorMessage="""
    //        La password deve avere tra 8 e 16 caratteri 
    //        E deve avere almeno: 
    //            - una lettera minuscola
    //            - una lettera maiuscola 
    //            - un numero.
    //        """)]
    //
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string Password { get; set; } = default;
        
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string Email { get; set; }
    
    [Editable(allowEdit: false, AllowInitialValue = true)]
    public string OldEmail { get; set; }

    //[StartDateRange(MINIMUM_YEAR, CURRENT_YEAR, MINIMUM_MONTH, MAXIMUM_MONTH, 
    //    ErrorMessage = """La data d'inizio non può essere oltre la data del giorno stesso.""")]
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime StartDate { get; set; }

    //[BirthDayRange(MAJOR_AGE, MAX_AGE,  
    //    ErrorMessage = $"""Il dipendente deve avere tra i 18 e i 55 anni.""")]
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime BirthDay { get; set; } 

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string NumericCode { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
   // [IsNotAdminRole(ADMIN_ROLE)]
    [EnumDataType(typeof(Roles))]
    public Roles Role { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public decimal? HourlyRate { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime? StartDateHourlyRate { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime? FinishDateHourlyRate { get; set; }

    [Editable(allowEdit: false, AllowInitialValue = true)]
    public bool IsDeleted { get; set; }

    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
}