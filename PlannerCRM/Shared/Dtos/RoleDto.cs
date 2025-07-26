using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class RoleDto
{
    public Guid? id { get; set; }
    public string? name { get; set; } = string.Empty;
    public bool? isRemoveable { get; set; } = false;
    //public ICollection<EmployeesRoleDto>? employeesRoles { get; set; }
}

public class RoleFilterDto : FilterDto
{
    public Guid? roleId { get; set; }
}

public partial class ApiUrl
{
    public const string ROLE_CONTROLLER = "api/role";

    public const string ROLE_INSERT = "insert";
    public const string ROLE_UPDATE = "update";
    public const string ROLE_DELETE = "delete";
    public const string ROLE_GET = "get";
    public const string ROLE_LIST = "list";
}