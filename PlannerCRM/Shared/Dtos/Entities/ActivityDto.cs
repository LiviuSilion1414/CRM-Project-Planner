namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public Guid  Guid { get; set; }

    [Required]
    [MinLength(5)]
    public string Name { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string CreationDateString { get => string.Format("{0:dd/MM/yyyy}", CreationDate); }

    [Required]
    [DateRangeValidation(nameof(StartDate), nameof(EndDate))]
    public DateTime StartDate { get; set; } = DateTime.Now;
    public string StartDateString { get => string.Format("{0:dd/MM/yyyy}", StartDate); }

    [Required]
    public DateTime EndDate { get; set; } = DateTime.Now;
    public string EndDateString { get => string.Format("{0:dd/MM/yyyy}", EndDate); }

    [Required]
    public Guid WorkOrderId { get; set; }
    
    [Required]
    public WorkOrderDto WorkOrder { get; set; }
}

public class ActivityFilterDto : FilterDto 
{
    public Guid ActivityId { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid ClientId { get; set; }
}

public partial class ApiUrl
{
    public const string ACTIVITY_CONTROLLER = "activity";

    public const string ACTIVITY_INSERT = "activity/insert";
    public const string ACTIVITY_UPDATE = "activity/update";
    public const string ACTIVITY_DELETE = "activity/delete";
    public const string ACTIVITY_GET = "activity/get";
    public const string ACTIVITY_LIST = "activity/list";

    public const string ACTIVITY_SEARCH = "activity/search";
    public const string ACTIVITY_FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID = "activity/findAssociatedEmployeesByActivityId";
    public const string ACTIVITY_FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID = "activity/findAssociatedWorkOrdersByActivityId";
    public const string ACTIVITY_FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY = "activity/findAssociatedWorkTimesWithinActivity";
}