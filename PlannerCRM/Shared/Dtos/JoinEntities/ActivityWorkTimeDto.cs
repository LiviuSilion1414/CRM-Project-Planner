namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ActivityWorkTimeDto
{
    public Guid  Guid { get; set; }
    public Guid ActivityId { get; set; }
    public Guid WorkTimeId { get; set; }

    // Navigation properties
    public ActivityDto Activity { get; set; }
    public WorkTimeDto WorkTime { get; set; }
}
