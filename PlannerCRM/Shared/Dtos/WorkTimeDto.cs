using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class WorkTimeDto
{
    public Guid? id { get; set; }
    public DateOnly? creationDate { get; set; }
    public int? hours { get; set; }
    public Guid? fkIdActivity { get; set; }
    public Guid? fkIdWorkOrder { get; set; }
    public Guid? fkIdFirmClient { get; set; }
    public ICollection<ActivitiesWorkTimeDto>? activitiesWorkTimes { get; set; }
    public ICollection<EmployeeWorkTimeDto>? employeeWorkTimes { get; set; }
    public ActivityDto? fkIdActivityNavigation { get; set; }
    public WorkOrderDto? fkIdWorkOrderNavigation { get; set; }
}

public class WorkTimeFilterDto : FilterDto
{
    public Guid? activityId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? employeeId { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
}

public partial class ApiUrl
{
    public const string WORKTIME_CONTROLLER = "api/workTime";

    public const string WORKTIME_INSERT = "insert";
    public const string WORKTIME_UPDATE = "update";
    public const string WORKTIME_DELETE = "delete";
    public const string WORKTIME_GET = "get";
    public const string WORKTIME_LIST = "list";
    public const string WORKTIME_FIND_ASSOCIATED_WORKTIMES_BY_EMPLOYEE_ID = "findAssociatedWorkTimesByEmployeeId";
}