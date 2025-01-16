namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ClientWorkOrderCostDto
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClient { get; set; }
    public WorkOrderCostDto WorkOrderCost { get; set; }
}
