using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class EmployeeActivity
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid ActivityId { get; set; }

    public virtual Activity Activity { get; set; }

    public virtual Employee Employee { get; set; }
}
