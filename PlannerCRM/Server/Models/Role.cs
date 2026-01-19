using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public bool? IsRemoveable { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<EmployeesRole> EmployeesRoles { get; set; } = new List<EmployeesRole>();

    public virtual ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
}
