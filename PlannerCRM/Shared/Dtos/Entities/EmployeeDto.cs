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

public partial class ApiUrl
{
    public const string EMPLOYEE_CONTROLLER = "api/employee";

    public const string EMPLOYEE_INSERT = "insert";
    public const string EMPLOYEE_UPDATE = "update";
    public const string EMPLOYEE_DELETE = "delete";
    public const string EMPLOYEE_GET = "get";
    public const string EMPLOYEE_LIST = "list";

    public const string EMPLOYEE_SEARCH = "search";
    public const string EMPLOYEE_SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY = "searchEmployeeByNameForRecovery";
    public const string EMPLOYEE_FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID = "findAssociatedActivitiesByEmployeeId";
}