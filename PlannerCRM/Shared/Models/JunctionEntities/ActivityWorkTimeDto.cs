using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;

public class ActivityWorkTimeDto
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public ActivityDto ActivityDto { get; set; }
    public WorkTimeDto WorkTimeDto { get; set; }
}
