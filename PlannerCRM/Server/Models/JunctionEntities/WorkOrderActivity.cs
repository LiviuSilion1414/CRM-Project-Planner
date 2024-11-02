namespace PlannerCRM.Server.Models.JunctionEntities;

using PlannerCRM.Server.Models.Entities;

public class WorkOrderActivity
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; }
    public Activity Activity { get; set; }
}
