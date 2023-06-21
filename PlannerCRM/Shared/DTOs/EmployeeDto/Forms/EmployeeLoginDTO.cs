using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeLoginDto
{
    public int Id { get; set; }

    [EmailAddress(ErrorMessage = "Indirizzo email non valido.")]
    [Required(ErrorMessage = """Campo "Email" richiesto""")]
    public string Email { get; set; }

    [Required(ErrorMessage = """Campo "Password" richiesto""")]
    [PasswordValidator(PASS_MIN_LENGTH, PASS_MAX_LENGTH, 
        ErrorMessage="La password deve avere tra 8 e 16 caratteri.")]
    public string Password { get; set; }

    public string Role { get; set; }
}