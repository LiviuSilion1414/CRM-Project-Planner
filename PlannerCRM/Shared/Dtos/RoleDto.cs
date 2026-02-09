using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;

public partial class RoleDto
{
    public Guid id { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Description("Name")]
    public string? name { get; set; } = string.Empty;
    public Guid? fkIdGroupRole { get; set; }

    public GroupRoleDto? fkIdGroupRoleNavigation { get; set; }
    public List<EmployeesRolesDto> employeeRolesList { get; set; } = new List<EmployeesRolesDto>();
    public List<MenuRoleDto> menuRoleList { get; set; } = new List<MenuRoleDto>();
}

public class RoleFilterDto : FilterDto
{
    public Guid? roleId { get; set; }
}
