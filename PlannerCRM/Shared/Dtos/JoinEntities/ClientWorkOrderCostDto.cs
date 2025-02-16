namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ClientWorkOrderCostDto
{
    public Guid  Guid { get; set; }
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClient { get; set; }
    public WorkOrderCostDto WorkOrderCost { get; set; }
}
