using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class ClientWorkOrder
{
    public Guid Id { get; set; }

    public Guid FirmClientId { get; set; }

    public Guid WorkOrderId { get; set; }
}
