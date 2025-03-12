namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderDto
{
    public Guid id { get; set; }
    
    [Required]
    [MinLength(5)]
    public string name { get; set; }

    public DateTime creationDate { get => DateTime.UtcNow; }
    public string creationDateString { get => string.Format("{0:dd/MM/yyyy}", creationDate); }

    [Required]
    public DateTime startDate { get; set; } = DateTime.Now;
    public string startDateString { get => string.Format("{0:dd/MM/yyyy}", startDate); }


    [Required]
    [DateRangeValidation(nameof(startDate), nameof(endDate))]
    public DateTime endDate { get; set; } = DateTime.Now;
    public string endDateString { get => string.Format("{0:dd/MM/yyyy}", endDate); }

    [Required]
    public Guid firmClientId { get; set; }
    
    public Guid workOrderCostId { get; set; }
   
    public FirmClientDto firmClient { get; set; }
    public List<ActivityDto> activities { get; set; }
}

public class WorkOrderFilterDto : FilterDto
{
    public Guid workOrderId { get; set; }
    public Guid firmClientId { get; set; }
}

public partial class ApiUrl
{
    public const string WORKORDER_CONTROLLER = "api/workorder";

    public const string WORKORDER_INSERT = "insert";
    public const string WORKORDER_UPDATE = "update";
    public const string WORKORDER_DELETE = "delete";
    public const string WORKORDER_GET = "get";
    public const string WORKORDER_LIST = "list";

    public const string WORKORDER_SEARCH = "search";
    public const string WORKORDER_FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID = "findAssociatedActivitiesByWorkOrderId";
    public const string WORKORDER_FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID = "findAssociatedWorkOrdersByClientId";
}