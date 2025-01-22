namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderDto
{
    public int Id { get; set; } = 0;

    [Required]
    [MinLength(8)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreationTime { get; set; } = DateTime.Now;

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required]
    [DateRangeValidation(nameof(StartDate), nameof(EndDate))]
    public DateTime EndDate { get; set; } = DateTime.Now;

    // Navigation properties
    [Required] 
    public int FirmClientId { get; set; } = 0;
    
    [Required] 
    public FirmClientDto FirmClient { get; set; } = new();
    
    public int WorkOrderCostId { get; set; } = 0;
    public WorkOrderCostDto WorkOrderCost { get; set; } = new();
    public List<ActivityDto> Activities { get; set; } = [];
}
