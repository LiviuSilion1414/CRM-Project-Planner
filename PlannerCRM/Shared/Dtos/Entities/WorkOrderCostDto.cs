namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderCostDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public decimal TotalCost { get; set; }
    public int WorkOrderId { get; set; }
    public int FirmClientId { get; set; }

    // Navigation properties
    public WorkOrderDto WorkOrderDto { get; set; }
    public FirmClientDto FirmClientDto { get; set; }
}
