using PlannerCRM.Shared.Attributes;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;


public class EmployeeFormDto
{
    public int Id { get; set; }

    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """Campo "Nome" richiesto""")]
    public string FirstName { get; set; }

    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """Campo "Cognome" richiesto""")]
    public string LastName { get; set; }

    [StrongPasswordValidator(PASS_MIN_LENGTH, PASS_MAX_LENGTH,
        ErrorMessage=""" Campo "Password" non valido. Riprovare. """)]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """Campo "Password" richiesto""")]
    public string Password { get; set; }

    [EmailAddress(ErrorMessage = """ Indirizzo "Email" non valido. """)]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """Campo "Email" richiesto""")]
    public string Email { get; set; }

    public string OldEmail { get; set; }

    public bool IsDeleted { get; set; }
    
    [StartDateRange(ErrorMessage = """La data d'inizio non può essere oltre la data di oggi.""")]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """ Campo "Data d'inizio" richiesto. """)]
    public DateTime? StartDate { get; set; }

    [BirthDayRange(MAJOR_AGE, MAX_AGE,  
        ErrorMessage = $"""Il dipendente deve avere tra i 18 e i 55 anni.""")]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """ Campo "Data di nascita" richiesto. """)]
    public DateTime? BirthDay { get; set; } 

    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """Campo "Codice fiscale" richiesto""")]
    public string NumericCode { get; set; }

    [IsNotAdminRole(ADMIN_ROLE)]
    [EnumDataType(typeof(Roles))]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """Campo "Ruolo" richiesto""")]
    public Roles? Role { get; set; }

    [MinimumHourlyRate(MINIMUM_HOURLY_RATE, ErrorMessage = $"""La tariffa oraria non può essere minore di 8€. """)]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """ Campo "Tariffa oraria" richiesto """)]
    public decimal? CurrentHourlyRate { get; set; }

    [StartDateRangeHourlyRate(MINIMUM_MONTH, MAXIMUM_MONTH, 
        ErrorMessage = """La data d'inizio non può essere oltre la data del giorno stesso.""")]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """ Campo "Data d'inizio tariffa oraria" richiesto """)]
    public DateTime? StartDateHourlyRate { get; set; }

    [FinishDateRangeHourlyRate(ErrorMessage = """La data di fine non può essere prima della data d'inizio.""")]
    [Editable(allowEdit: true)]
    [Required(ErrorMessage = """ Campo "Data di fine tariffa oraria" richiesto """)]
    public DateTime? FinishDateHourlyRate { get; set; }

    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
}