namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderDto
{
    public int Id { get; set; }
    

    [Required]
    [MinLength(8)]
    public string Name { get; set; }

    public DateTime CreationTime { get => DateTime.UtcNow; }

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required]
    [DateRangeValidation(nameof(StartDate), nameof(EndDate))]
    public DateTime EndDate { get; set; } = DateTime.Now;

    // Navigation properties
    [Required]
    public int FirmClientId { get; set; }
    
    public int WorkOrderCostId { get; set; }
   
    [Required]
    public FirmClientDto FirmClient { get; set; }
    public WorkOrderCostDto WorkOrderCost { get; set; }
    public List<ActivityDto> Activities { get; set; }
}
