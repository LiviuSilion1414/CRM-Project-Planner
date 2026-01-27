using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class EmployeesRolesDto
{
    public Guid? id { get; set; }
    public Guid fkIdEmployee { get; set; }
    public Guid fkIdRole { get; set; }
    public bool? isRemoveable { get; set; }
    public EmployeeDto? fkIdEmployeeNavigation { get; set; }
    public RoleDto? fkIdRoleNavigation { get; set; }
}

public class EmployeesRolesFilterDto : FilterDto
{
    public Guid? employeeId { get; set; }
    public Guid? roleId { get; set; }
}