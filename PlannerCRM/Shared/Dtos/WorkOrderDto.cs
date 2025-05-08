using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class WorkOrderDto
{
    public Guid? id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public DateOnly? creationDate { get; set; }
    public DateOnly? startDate { get; set; }
    public DateOnly? endDate { get; set; }
    public Guid? fkIdFirmClient { get; set; }
    public ICollection<ActivityDto>? activities { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public ICollection<FirmClientsWorkOrderDto>? firmClientsWorkOrders { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public ICollection<WorkOrdersActivityDto>? workOrdersActivities { get; set; }
    public ICollection<WorkTimeDto>? workTimes { get; set; }
}

public class WorkOrderFilterDto : FilterDto
{
    public Guid? workOrderId { get; set; }
    public Guid? firmClientId { get; set; }
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
