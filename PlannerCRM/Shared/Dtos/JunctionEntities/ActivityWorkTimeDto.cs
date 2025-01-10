using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;

public class ActivityWorkTimeDto
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public ActivityDto Activity { get; set; }
    public WorkTimeDto WorkTime { get; set; }
}
