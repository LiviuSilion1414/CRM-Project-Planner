using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class WorkOrderActivity
{
    public Guid Id { get; set; }

    public Guid WorkOrderId { get; set; }

    public Guid ActivityId { get; set; }

    public virtual Activity Activity { get; set; }

    public virtual WorkOrder WorkOrder { get; set; }
}
