using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class EmployeeActivity
{
    public Guid Id { get; set; }

    public Guid FkIdActivity { get; set; }

    public Guid FkIdEmployee { get; set; }

    public Guid FkIdWorkOrder { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public virtual Activity FkIdActivityNavigation { get; set; }

    public virtual Employee FkIdEmployeeNavigation { get; set; }

    public virtual WorkOrder FkIdWorkOrder1 { get; set; }

    public virtual FirmClient FkIdWorkOrderNavigation { get; set; }
}
