namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderCostDto
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public decimal TotalCost { get; set; } = 0M;
    public int WorkOrderId { get; set; } = 0;
    public int FirmClientId { get; set; } = 0;

    // Navigation properties
    public WorkOrderDto WorkOrder { get; set; } = new();
    public FirmClientDto FirmClient { get; set; } = new();
}
