using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class WorkOrder
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? FkIdFirmClient { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual FirmClient FkIdFirmClientNavigation { get; set; }
}
