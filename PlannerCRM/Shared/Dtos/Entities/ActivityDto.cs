namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public Guid id { get; set; }

    [Required]
    [MinLength(5)]
    public string name { get; set; }

    [Required]
    public DateTime creationDate { get; set; } = DateTime.Now;
    public string creationDateString { get => string.Format("{0:dd/MM/yyyy}", creationDate); }

    [Required]
    [DateRangeValidation(nameof(startDate), nameof(endDate))]
    public DateTime startDate { get; set; } = DateTime.Now;
    public string startDateString { get => string.Format("{0:dd/MM/yyyy}", startDate); }

    [Required]
    public DateTime endDate { get; set; } = DateTime.Now;
    public string endDateString { get => string.Format("{0:dd/MM/yyyy}", endDate); }

    [Required]
    public Guid workOrderId { get; set; }
    
    [Required]
    public WorkOrderDto workOrder { get; set; }
}

public class ActivityFilterDto : FilterDto 
{
    public Guid activityId { get; set; }
    public Guid workOrderId { get; set; }
    public Guid clientId { get; set; }
}

public partial class ApiUrl
{
    public const string ACTIVITY_CONTROLLER = "api/activity";

    public const string ACTIVITY_INSERT = "insert";
    public const string ACTIVITY_UPDATE = "update";
    public const string ACTIVITY_DELETE = "delete";
    public const string ACTIVITY_GET = "get";
    public const string ACTIVITY_LIST = "list";

    public const string ACTIVITY_SEARCH = "search";
    public const string ACTIVITY_FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID = "findAssociatedEmployeesByActivityId";
    public const string ACTIVITY_FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID = "findAssociatedWorkOrdersByActivityId";
    public const string ACTIVITY_FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY = "findAssociatedWorkTimesWithinActivity";
}