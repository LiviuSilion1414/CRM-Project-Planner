using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class EmployeeActivity
{
    public Guid Id { get; set; }

    public Guid FkIdActivity { get; set; }

    public Guid FkIdEmployee { get; set; }

    public virtual Activity FkIdActivityNavigation { get; set; }

    public virtual Employee FkIdEmployeeNavigation { get; set; }
}
