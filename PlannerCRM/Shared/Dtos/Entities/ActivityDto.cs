namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    public string Name { get; set; }

    [Required]
    [PresentOrFutureDate]
    public DateTime CreationDate { get; set; }

    [Required]
    [PresentOrFutureDate]
    public DateTime StartDate { get; set; }

    [Required]
    [PresentOrFutureDate]
    public DateTime EndDate { get; set; }

    [Required]
    public int WorkOrderId { get; set; }

    // Navigation properties
    [Required]
    public WorkOrderDto WorkOrderDto { get; set; }
    public ICollection<EmployeeDto> EmployeesDto { get; set; }
    public ICollection<EmployeeActivityDto> EmployeeActivitiesDto { get; set; }
    public ICollection<ActivityWorkTimeDto> ActivityWorkTimesDto { get; set; }
}
