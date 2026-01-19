using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class FirmClientDto
{
    public Guid? id { get; set; }
    public string? name { get; set; }
    public string? vatNumber { get; set; }
    public string? email { get; set; }
    public string? fiscalCode { get; set; }
    public ICollection<ActivityDto>? activities { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public ICollection<WorkOrderDto>? workOrders { get; set; }
}

public class FirmClientFilterDto : FilterDto
{
    public Guid? firmClientId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? workOrderCostId { get; set; }
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