namespace PlannerCRM.Shared.Dtos.Entities;

public class FirmClientDto
{
    public Guid  Guid { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Length(8, 15)]
    [Required]
    public string VatNumber { get; set; }
    public List<WorkOrderDto> WorkOrders { get; set; }
}

public class FirmClientFilterDto : FilterDto
{
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid WorkOrderCostId { get; set; }
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