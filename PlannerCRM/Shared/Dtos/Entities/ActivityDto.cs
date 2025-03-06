namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public Guid  Guid { get; set; }

    [Required]
    [MinLength(5)]
    public string Name { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string CreationDateString { get => string.Format("{0:dd/MM/yyyy}", CreationDate); }

    [Required]
    [DateRangeValidation(nameof(StartDate), nameof(EndDate))]
    public DateTime StartDate { get; set; } = DateTime.Now;
    public string StartDateString { get => string.Format("{0:dd/MM/yyyy}", StartDate); }

    [Required]
    public DateTime EndDate { get; set; } = DateTime.Now;
    public string EndDateString { get => string.Format("{0:dd/MM/yyyy}", EndDate); }

    [Required]
    public Guid WorkOrderId { get; set; }
    
    [Required]
    public WorkOrderDto WorkOrder { get; set; }
}

public class ActivityFilterDto : FilterDto 
{
    public Guid ActivityId { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid ClientId { get; set; }
}