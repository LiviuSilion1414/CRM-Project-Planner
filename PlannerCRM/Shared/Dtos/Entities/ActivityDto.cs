namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public int Id { get; set; } = 0;

    [Required]
    [MinLength(5)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required]
    [DateRangeValidation(nameof(StartDate), nameof(EndDate))]
    public DateTime EndDate { get; set; } = DateTime.Now;

    // Navigation properties
    [Required]
    public int WorkOrderId { get; set; } = 0;

    [Required]
    public WorkOrderDto WorkOrder { get; set; } = new();

    public List<EmployeeDto> Employees { get; set; } = [];
    public List<EmployeeActivityDto> EmployeeActivities { get; set; } = [];
    public List<ActivityWorkTimeDto> ActivityWorkTimes { get; set; } = [];
}
