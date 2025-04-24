using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class EmployeesRole
{
    public Guid Id { get; set; }

    public Guid FkIdEmployee { get; set; }

    public Guid FkIdRole { get; set; }

    public string RoleName { get; set; }

    public bool IsRemoveable { get; set; }

    public virtual Employee FkIdEmployeeNavigation { get; set; }

    public virtual Role FkIdRoleNavigation { get; set; }
}
