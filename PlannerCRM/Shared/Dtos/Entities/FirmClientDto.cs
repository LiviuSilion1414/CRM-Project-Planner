namespace PlannerCRM.Shared.Dtos.Entities;

public class FirmClientDto
{
    public Guid id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string name { get; set; }
    
    [Length(8, 15)]
    [Required]
    public string vatNumber { get; set; }
    public List<WorkOrderDto> workOrders { get; set; }
}

public class FirmClientFilterDto : FilterDto
{
    public Guid firmClientId { get; set; }
    public Guid workOrderId { get; set; }
    public Guid workOrderCostId { get; set; }
}

public partial class ApiUrl
{
    public const string CLIENT_CONTROLLER = "api/client";

    public const string CLIENT_INSERT = "insert";
    public const string CLIENT_UPDATE = "update";
    public const string CLIENT_DELETE = "delete";
    public const string CLIENT_GET = "get";
    public const string CLIENT_LIST = "list";
    public const string CLIENT_SEARCH = "search";
    public const string CLIENT_FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID = "findAssociatedWorkOrdersByClientId";
}