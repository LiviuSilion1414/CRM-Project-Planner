using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class EmployeeWorkTime
{
    public Guid Id { get; set; }

    public Guid FkIdEmployee { get; set; }

    public Guid FkIdWorkTime { get; set; }

    public Guid FkIdActivity { get; set; }

    public Guid FkIdWorkOrder { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public virtual Employee FkIdEmployeeNavigation { get; set; }

    public virtual FirmClient FkIdFirmClientNavigation { get; set; }

    public virtual WorkTime FkIdWorkTimeNavigation { get; set; }
}
