using PlannerCRM.Shared.Dtos.JunctionEntities;

namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int WorkOrderId { get; set; }

    // Navigation properties
    public WorkOrderDto WorkOrderDto { get; set; }
    public ICollection<EmployeeDto> EmployeesDto { get; set; }
    public ICollection<EmployeeActivityDto> EmployeeActivitiesDto { get; set; }
    public ICollection<ActivityWorkTimeDto> ActivityWorkTimesDto { get; set; }
}
