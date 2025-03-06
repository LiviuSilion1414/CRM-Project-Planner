namespace PlannerCRM.Server.Models.JoinEntities;

public class WorkOrderActivity
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid ActivityId { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; }
    public Activity Activity { get; set; }
}
