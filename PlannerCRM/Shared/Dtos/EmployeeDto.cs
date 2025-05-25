using PlannerCRM.Client.Services;
using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class EmployeeDto
{
    public Guid? id { get; set; }
    public string? name { get; set; }
    public string? surname { get; set; }
    public string? username { get; set; }
    public string? fullname => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname) ? surname + " " + name : string.Empty;
    public DateTime? creationDate { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "not set";
    public string? passwordHash { get; set; }
    public DateTime? lastSeen { get; set; }
    public string? lastSeenString => lastSeen != null ? string.Format("{0:dd/MM/yyyy}", lastSeen) : "not set";
    public bool? isRemoveable { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public ICollection<EmployeesRoleDto>? employeesRoles { get; set; }
    public string employeesRolesCommaSeparatedString => employeesRoles != null && employeesRoles.Any()
        ? string.Join(", ", employeesRoles.Select(er => er.roleName))
        : "no roles set";
}

public class EmployeeFilterDto : FilterDto
{
    public Guid? employeeId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? activityId { get; set; }
    public Guid? roleId { get; set; }
    public bool? isRemoveRole { get; set; }
    public RoleDto? role { get; set; }
}

    public partial class ApiUrl
{
    public const string EMPLOYEE_CONTROLLER = "api/employee";

    public const string EMPLOYEE_INSERT = "insert";
    public const string EMPLOYEE_UPDATE = "update";
    public const string EMPLOYEE_DELETE = "delete";
    public const string EMPLOYEE_GET = "get";
    public const string EMPLOYEE_LIST = "list";

    public const string EMPLOYEE_UPDATE_ROLE = "updateRole";
    public const string EMPLOYEE_SEARCH = "search";
    public const string EMPLOYEE_SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY = "searchEmployeeByNameForRecovery";
    public const string EMPLOYEE_FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID = "findAssociatedActivitiesByEmployeeId";
}