using PlannerCRM.Shared.Models.JunctionEntities;

namespace PlannerCRM.Shared.Models.Entities;

public class WorkTimeDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public int WorkOrderId { get; set; }
    public int EmployeeId { get; set; }
    public double WorkedHours { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public WorkOrderDto WorkOrderDto { get; set; }
    public EmployeeDto EmployeeDto { get; set; }
    public ActivityDto ActivityDto { get; set; }
    public ICollection<ActivityWorkTimeDto> ActivityWorkTimesDto { get; set; }
}
