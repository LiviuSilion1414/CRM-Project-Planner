using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class RoleDto
{
    public Guid id { get; set; }
    public string? name { get; set; } = string.Empty;
    public bool isRemoveable { get; set; } = false;
    public string isRemoveableString => isRemoveable ? "Yes" : "No";

    public List<EmployeesRolesDto> employeeRolesList { get; set; } = new List<EmployeesRolesDto>();
    public List<MenuRoleDto> menuRoleList { get; set; } = new List<MenuRoleDto>();
}

public class RoleFilterDto : FilterDto
{
    public Guid? roleId { get; set; }
}
