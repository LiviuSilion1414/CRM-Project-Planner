namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ClientWorkOrderDto
{
    public int Id { get; set; } = 0;
    public int FirmClientId { get; set; } = 0;
    public int WorkOrderId { get; set; } = 0;

    // Navigation properties
    public FirmClientDto FirmClient { get; set; } = new();
    public WorkOrderDto WorkOrder { get; set; } = new();
}
