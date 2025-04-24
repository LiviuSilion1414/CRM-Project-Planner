using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class FirmClientsWorkOrder
{
    public Guid Id { get; set; }

    public Guid FkIdFirmClient { get; set; }

    public Guid FkIdWorkOrder { get; set; }

    public virtual FirmClient FkIdFirmClientNavigation { get; set; }

    public virtual WorkOrder FkIdWorkOrderNavigation { get; set; }
}
