using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class MenuRoleDto
{
    public Guid id { get; set; }

    public Guid fkIdMenu { get; set; }

    public Guid fkIdRole { get; set; }

    public virtual MenuDto fkIdMenuNavigation { get; set; }

    public virtual RoleDto fkIdRoleNavigation { get; set; }
    public ICollection<EmployeesRolesDto>? employeesRoles { get; set; }
}

public partial class MenuRoleFilterDto : FilterDto
{
    public Guid? menuId { get; set; }
    public Guid? roleId { get; set; }
    public List<Guid>? idList { get; set; }

}
