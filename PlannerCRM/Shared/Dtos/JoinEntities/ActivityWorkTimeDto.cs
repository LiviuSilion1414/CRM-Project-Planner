namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ActivityWorkTimeDto
{
    public int Id { get; set; } = 0;
    public int ActivityId { get; set; } = 0;
    public int WorkTimeId { get; set; } = 0;

    // Navigation properties
    public ActivityDto Activity { get; set; } = new();
    public WorkTimeDto WorkTime { get; set; } = new();
}
