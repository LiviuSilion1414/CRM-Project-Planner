using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class EmployeeWorkTime
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid WorkTimeId { get; set; }

    public Guid WorkOrderId { get; set; }

    public Guid ActivityId { get; set; }

    public virtual Activity Activity { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual WorkOrder WorkOrder { get; set; }

    public virtual WorkTime WorkTime { get; set; }
}
