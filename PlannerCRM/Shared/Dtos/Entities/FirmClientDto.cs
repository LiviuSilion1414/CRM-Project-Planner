namespace PlannerCRM.Shared.Dtos.Entities;

public class FirmClientDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Length(11, 11)]
    [Required]
    public string VatNumber { get; set; }

    // Navigation properties
    public List<WorkOrderDto> WorkOrders { get; set; }
    public List<WorkOrderCostDto> WorkOrderCosts { get; set; }
}
