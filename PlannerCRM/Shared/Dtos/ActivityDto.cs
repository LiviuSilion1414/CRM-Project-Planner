using PlannerCRM.Client.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace PlannerCRM.Shared.Dtos;

public partial class ActivityDto
{
    public Guid? id { get; set; }
    public string? name { get; set; }
    public DateTime? creationDate { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "not set";
    public DateTime? startDate { get; set; }
    public string? startDateString => startDate != null ? string.Format("{0:dd/MM/yyyy}", startDate) : "not set";
    public DateTime? endDate { get; set; }
    public string? endDateString => endDate != null ? string.Format("{0:dd/MM/yyyy}", endDate) : "not set";
    public Guid? fkIdWorkOrder { get; set; }
    public Guid? fkIdFirmClient { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public WorkOrderDto? fkIdWorkOrderNavigation { get; set; }
    public ICollection<WorkTimeDto>? workTimes { get; set; }
}

public class ActivityFilterDto : FilterDto
{
    public Guid? activityId { get; set; }
    public Guid? employeeId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? firmClientId { get; set; }
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
    public const string ACTIVITY_ASSIGN_ACTIVITY = "assignActivity";
    public const string ACTIVITY_REMOVE_ASSIGNED_EMPLOYEE = "removeAssignedEmployee";
}