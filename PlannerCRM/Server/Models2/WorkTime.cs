using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models2;

public partial class WorkTime
{
    public Guid Id { get; set; }

    public DateOnly CreationDate { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid ActivityId { get; set; }

    public Guid WorkOrderId { get; set; }

    public int Hours { get; set; }

    public string Name { get; set; }

    public virtual Activity Activity { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual WorkOrder WorkOrder { get; set; }
}
