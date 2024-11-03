using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;

public class ClientWorkOrderDto
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClientDto { get; set; }
    public WorkOrderDto WorkOrderDto { get; set; }
}
