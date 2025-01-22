namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class WorkOrderActivityDto
{
    public int Id { get; set; } = 0;
    public int WorkOrderId { get; set; } = 0;
    public int ActivityId { get; set; } = 0;
 
    // Navigation properties
    public WorkOrderDto WorkOrder { get; set; } = new();
    public ActivityDto Activity { get; set; } = new();
}
