using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string RoleName { get; set; }

    public bool? IsRemoveable { get; set; }

    public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
}
