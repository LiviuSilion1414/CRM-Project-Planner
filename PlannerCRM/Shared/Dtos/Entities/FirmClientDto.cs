namespace PlannerCRM.Shared.Dtos.Entities;

public class FirmClientDto
{
    public int Id { get; set; } = 0;

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Length(8, 15)]
    [Required]
    public string VatNumber { get; set; } = string.Empty;
    public List<WorkOrderDto> WorkOrders { get; set; } = [];

    // Navigation properties
    public List<WorkOrderCostDto> WorkOrderCosts { get; set; } = [];
}
