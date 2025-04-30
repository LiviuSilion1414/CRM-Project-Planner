using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Activity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid FkIdWorkOrder { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public virtual FirmClient FkIdFirmClientNavigation { get; set; }

    public virtual WorkOrder FkIdWorkOrderNavigation { get; set; }

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
