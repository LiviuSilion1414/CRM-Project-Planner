namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderDto
{
    public Guid  Guid { get; set; }
    

    [Required]
    [MinLength(8)]
    public string Name { get; set; }

    public DateTime CreationDate { get => DateTime.UtcNow; }
    public string CreationDateString { get => string.Format("{0:dd/MM/yyyy}", CreationDate); }

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Now;
    public string StartDateString { get => string.Format("{0:dd/MM/yyyy}", StartDate); }


    [Required]
    [DateRangeValidation(nameof(StartDate), nameof(EndDate))]
    public DateTime EndDate { get; set; } = DateTime.Now;
    public string EndDateString { get => string.Format("{0:dd/MM/yyyy}", EndDate); }

    [Required]
    public Guid FirmClientId { get; set; }
    
    public Guid WorkOrderCostId { get; set; }
   
    [Required]
    public FirmClientDto FirmClient { get; set; }
    public List<ActivityDto> Activities { get; set; }
}

public class WorkOrderFilterDto : FilterDto
{
    public Guid WorkOrderId { get; set; }
    public Guid FirmClientId { get; set; }
}