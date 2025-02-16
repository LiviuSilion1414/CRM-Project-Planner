namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class WorkOrderActivityDto
{
    public Guid  Guid { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid ActivityId { get; set; }

    // Navigation properties
    public WorkOrderDto WorkOrder { get; set; }
    public ActivityDto Activity { get; set; }
}
