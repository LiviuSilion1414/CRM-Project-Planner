using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models2;

public partial class WorkOrder
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly CreationTime { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid FirmClientId { get; set; }

    public Guid WorkOrderCostId { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual FirmClient FirmClient { get; set; }

    public virtual ICollection<WorkOrderActivity> WorkOrderActivities { get; set; } = new List<WorkOrderActivity>();

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
