using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class WorkOrder
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ActivitiesWorkTime> ActivitiesWorkTimes { get; set; } = new List<ActivitiesWorkTime>();

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual ICollection<FirmClientsWorkOrder> FirmClientsWorkOrders { get; set; } = new List<FirmClientsWorkOrder>();

    public virtual FirmClient FkIdFirmClientNavigation { get; set; }

    public virtual ICollection<WorkOrdersActivity> WorkOrdersActivities { get; set; } = new List<WorkOrdersActivity>();

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
