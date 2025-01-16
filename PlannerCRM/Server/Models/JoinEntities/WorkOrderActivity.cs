namespace PlannerCRM.Server.Models.JoinEntities;

public class WorkOrderActivity
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; }
    public Activity Activity { get; set; }
}
