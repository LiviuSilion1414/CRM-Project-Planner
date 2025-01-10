using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;

public class WorkOrderActivityDto
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public WorkOrderDto WorkOrder { get; set; }
    public ActivityDto Activity { get; set; }
}
