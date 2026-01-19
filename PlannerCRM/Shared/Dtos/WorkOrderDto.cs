using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class WorkOrderDto
{
    public Guid? id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public DateTime? creationDate { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "not set";
    public DateTime? startDate { get; set; }
    public string? startDateString => startDate != null ? string.Format("{0:dd/MM/yyyy}", startDate) : "not set";
    public DateTime? endDate { get; set; }
    public string? endDateString => endDate != null ? string.Format("{0:dd/MM/yyyy}", endDate) : "not set";
    public Guid? fkIdFirmClient { get; set; }
    public ICollection<ActivityDto>? activities { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public ICollection<WorkTimeDto>? workTimes { get; set; }
}

public class WorkOrderFilterDto : FilterDto
{
    public Guid? workOrderId { get; set; }
    public Guid? firmClientId { get; set; }
    public Guid? employeeId { get; set; }
}

public partial class ApiUrl
{
    public const string WORKORDER_CONTROLLER = "api/workorder";

    public const string WORKORDER_INSERT = "insert";
    public const string WORKORDER_UPDATE = "update";
    public const string WORKORDER_DELETE = "delete";
    public const string WORKORDER_GET = "get";
    public const string WORKORDER_LIST = "list";
}
