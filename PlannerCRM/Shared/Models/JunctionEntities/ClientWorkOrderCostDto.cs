using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;

public class ClientWorkOrderCostDto
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClientDto { get; set; }
    public WorkOrderCostDto WorkOrderCostDto { get; set; }
}
