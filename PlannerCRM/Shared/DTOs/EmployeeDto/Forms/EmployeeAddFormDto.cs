using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.Models;
using System.ComponentModel;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeAddFormDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = """Campo "Nome" richiesto""")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = """Campo "Cognome" richiesto""")]
    public string LastName { get; set; }

    [RangeValidator(PASS_MIN_LENGTH, PASS_MAX_LENGTH, 
        ErrorMessage="La password deve avere tra 8 e 16 caratteri.")]
    [Required(ErrorMessage = """Campo "Password" richiesto""")]
    public string Password { get; set; }

    [EmailAddress(ErrorMessage = """ Campo "Email" non valido. """)]
    [Required(ErrorMessage = """Campo "Email" richiesto""")]
    public string Email { get; set; }

    [Required(ErrorMessage = """ Campo "Data d'inizio" richiesto. """)]
    //[EmployeeStartDateRange(MINIMUM_YEAR, CURRENT_YEAR, ErrorMessage = "La data dev'essere tra il 01/01/1973 e l'anno del giorno stesso")]
    public DateTime? StartDate { get; set; }
    
    [Required(ErrorMessage = """ Campo "Data di nascita" richiesto. """)]
    //[BirthDayRange(ErrorMessage = """L'età dell'impiegato deve essere tra i 18 e i 50 anni.""")]
    public DateTime? BirthDay { get; set; } 

    [Required(ErrorMessage = """Campo "Codice fiscale" richiesto""")]
    public string NumericCode { get; set; }

    [Required(ErrorMessage = """Campo "Ruolo" richiesto""")]
    [EnumDataType(typeof(Roles))]
    public Roles? Role { get; set; }

    [Required(ErrorMessage = """ Campo "Tariffa oraria" richiesto """)]
    public float? HourlyRate { get; set; }

    [Required(ErrorMessage = """ Campo "Data d'inizio tariffa oraria" richiesto """)]
    public DateTime? StartDateHourlyRate { get; set; }

    [Required(ErrorMessage = """ Campo "Data di fine tariffa oraria" richiesto """)]
    public DateTime? FinishDateHourlyRate { get; set; }

    //[Required(ErrorMessage = """Campo "Tariffa oraria" richiesto""")]
    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }

    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}