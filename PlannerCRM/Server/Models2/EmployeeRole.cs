using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models2;

public partial class EmployeeRole
{
    public Guid Id { get; set; }

    public string RoleName { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid RoleId { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual Role Role { get; set; }
}
