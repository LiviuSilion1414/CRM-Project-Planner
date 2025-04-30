using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class FirmClient
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VatNumber { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ActivitiesWorkTime> ActivitiesWorkTimes { get; set; } = new List<ActivitiesWorkTime>();

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual ICollection<FirmClientsWorkOrder> FirmClientsWorkOrders { get; set; } = new List<FirmClientsWorkOrder>();

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

    public virtual ICollection<WorkOrdersActivity> WorkOrdersActivities { get; set; } = new List<WorkOrdersActivity>();
}
