namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkTimeDto
{
    public int Id { get; set; } = 0;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public double WorkedHours { get; set; } = 0D;
    public int WorkOrderId { get; set; } = 0;
    public int EmployeeId { get; set; } = 0;
    public int ActivityId { get; set; } = 0;

    // Navigation properties
    public WorkOrderDto WorkOrder { get; set; } = new();
    public EmployeeDto Employee { get; set; } = new();
    public ActivityDto Activity { get; set; } = new();
    public List<ActivityWorkTimeDto> ActivityWorkTimes { get; set; } = [];
}
