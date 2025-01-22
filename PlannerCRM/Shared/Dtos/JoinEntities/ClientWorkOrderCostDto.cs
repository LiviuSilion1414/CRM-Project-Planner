namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ClientWorkOrderCostDto
{
    public int Id { get; set; } = 0;
    public int FirmClientId { get; set; } = 0;
    public int WorkOrderCostId { get; set; } = 0;

    // Navigation properties
    public FirmClientDto FirmClient { get; set; } = new();
    public WorkOrderCostDto WorkOrderCost { get; set; } = new();
}
