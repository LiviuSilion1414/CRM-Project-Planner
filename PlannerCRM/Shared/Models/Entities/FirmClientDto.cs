namespace PlannerCRM.Shared.Models.Entities;

public class FirmClientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    // Navigation properties
    public ICollection<WorkOrderDto> WorkOrdersDto { get; set; }
    public ICollection<WorkOrderCostDto> WorkOrderCostsDto { get; set; }
}
