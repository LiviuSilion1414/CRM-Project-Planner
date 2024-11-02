namespace PlannerCRM.Server.Models.JunctionEntities;

using PlannerCRM.Server.Models.Entities;

public class ActivityWorkTime
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public Activity Activity { get; set; }
    public WorkTime WorkTime { get; set; }
}
