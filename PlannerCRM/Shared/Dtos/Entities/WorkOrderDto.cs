namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderDto
{
    public int Id { get; set; }
    

    [Required]
    [MinLength(8)]
    public string Name { get; set; }

    public DateTime CreationTime { get => DateTime.UtcNow; }
    
    [Required]
    [PresentOrFutureDate]
    public DateTime StartDate { get; set; }

    [Required]
    [PresentOrFutureDate]
    public DateTime EndDate { get; set; }
    
    [Required]
    public int FirmClientId { get; set; }
    
    public int WorkOrderCostId { get; set; }
    [Required]

    public FirmClientDto FirmClient { get; set; }
    public List<ActivityDto> Activities { get; set; }

    // Navigation properties
    //[Required]
    //public WorkOrderCostDto WorkOrderCost { get; set; }
}
