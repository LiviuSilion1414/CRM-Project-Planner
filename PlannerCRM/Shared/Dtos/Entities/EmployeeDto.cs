namespace PlannerCRM.Shared.Dtos.Entities;

public class EmployeeDto
{
    public Guid id { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "The name should be at least {0} characters")]
    public string name { get; set; }

    [Required]
    [Phone]
    public string phone { get; set; }
    
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    [Required]
    public string password { get; set; }

    public List<RoleDto> roles { get; set; }
}

public class EmployeeFilterDto : FilterDto
{
    public Guid employeeId { get; set; }
    public Guid activityId { get; set; }
    public Guid workTimeId { get; set; }
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