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

    public string HexColor { get; set; }

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual WorkOrder FkIdWorkOrderNavigation { get; set; }

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
