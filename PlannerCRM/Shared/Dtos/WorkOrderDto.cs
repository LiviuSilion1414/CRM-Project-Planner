using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace PlannerCRM.Shared.Dtos;

public class WorkOrderDto
{
    public Guid id { get; set; }
    
    [Required]
    [MinLength(5)]
    public string name { get; set; }

    [Required]
    [MinLength(5)]
    public string description { get; set; }

    public DateOnly creationDate { get => DateOnly.FromDateTime(DateTime.Now); }
    public string creationDateString { get => string.Format("{0:dd/MM/yyyy}", creationDate); }

    [Required]
    public DateOnly startDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string startDateString { get => string.Format("{0:dd/MM/yyyy}", startDate); }


    [Required]
    [DateRangeValidation(nameof(startDate), nameof(endDate))]
    public DateOnly endDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string endDateString { get => string.Format("{0:dd/MM/yyyy}", endDate); }

    [Required(ErrorMessage = "The firm client is required")]
    public Guid? firmClientId { get; set; }
    
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