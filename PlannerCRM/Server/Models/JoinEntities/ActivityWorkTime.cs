namespace PlannerCRM.Server.Models.JoinEntities;

using PlannerCRM.Server.Models.Entities;

public class ActivityWorkTime
{
    public Guid Guid { get; set; }
    public Guid ActivityId { get; set; }
    public Guid WorkTimeId { get; set; }

    // Navigation properties
    public Activity Activity { get; set; }
    public WorkTime WorkTime { get; set; }
}
