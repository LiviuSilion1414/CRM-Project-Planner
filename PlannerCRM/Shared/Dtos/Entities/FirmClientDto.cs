namespace PlannerCRM.Shared.Dtos.Entities;

public class FirmClientDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    public string VatNumber { get; set; }

    // Navigation properties
    public ICollection<WorkOrderDto> WorkOrdersDto { get; set; }
    public ICollection<WorkOrderCostDto> WorkOrderCostsDto { get; set; }
}
