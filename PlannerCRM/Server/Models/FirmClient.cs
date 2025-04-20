using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class FirmClient
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VatNumber { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
