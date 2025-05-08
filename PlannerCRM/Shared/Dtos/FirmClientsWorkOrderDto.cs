using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class FirmClientsWorkOrderDto
{
    public Guid? id { get; set; }
    public Guid? fkIdFirmClient { get; set; }
    public Guid? fkIdWorkOrder { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public WorkOrderDto? fkIdWorkOrderNavigation { get; set; }
}
