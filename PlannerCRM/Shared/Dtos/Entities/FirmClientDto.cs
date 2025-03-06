namespace PlannerCRM.Shared.Dtos.Entities;

public class FirmClientDto
{
    public Guid  Guid { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Length(8, 15)]
    [Required]
    public string VatNumber { get; set; }
    public List<WorkOrderDto> WorkOrders { get; set; }
}

public class FirmClientFilterDto : FilterDto
{
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid WorkOrderCostId { get; set; }
}