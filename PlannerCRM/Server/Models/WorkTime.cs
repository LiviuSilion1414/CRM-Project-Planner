using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class WorkTime
{
    public Guid Id { get; set; }

    public DateOnly CreationDate { get; set; }

    public int Hours { get; set; }

    public Guid FkIdActivity { get; set; }

    public Guid FkIdWorkOrder { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public virtual ICollection<ActivitiesWorkTime> ActivitiesWorkTimes { get; set; } = new List<ActivitiesWorkTime>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual Activity FkIdActivityNavigation { get; set; }

    public virtual WorkOrder FkIdWorkOrderNavigation { get; set; }
}
