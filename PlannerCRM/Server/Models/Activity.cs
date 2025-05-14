using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Activity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid FkIdWorkOrder { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual ICollection<WorkOrdersActivity> WorkOrdersActivities { get; set; } = new List<WorkOrdersActivity>();

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
