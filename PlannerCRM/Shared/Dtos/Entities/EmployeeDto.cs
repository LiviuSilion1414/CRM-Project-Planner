namespace PlannerCRM.Shared.Dtos.Entities;

public class EmployeeDto
{
    public Guid  Guid { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "The name should be at least {0} characters")]
    public string Name { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [PasswordValidator]
    public string Password { get; set; }
}

public class EmployeeFilterDto : FilterDto
{
    public Guid EmployeeId { get; set; }
    public Guid ActivityId { get; set; }
    public Guid WorkTimeId { get; set; }
    public Guid SalaryId { get; set; }
}