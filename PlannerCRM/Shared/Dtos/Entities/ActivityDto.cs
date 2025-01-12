namespace PlannerCRM.Shared.Dtos.Entities;

public class ActivityDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    public string Name { get; set; }

    [Required]
    [PresentOrFutureDate]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [Required]
    [PresentOrFutureDate]
    public DateTime StartDate { get; set; }

    [Required]
    [PresentOrFutureDate]
    public DateTime EndDate { get; set; }

    [Required]
    public int WorkOrderId { get; set; }

    //// Navigation properties
    //[Required]
    //public WorkOrderDto WorkOrder { get; set; }
    //public List<EmployeeDto> Employees { get; set; }
    //public List<EmployeeActivityDto> EmployeeActivities { get; set; }
    //public List<ActivityWorkTimeDto> ActivityWorkTimes { get; set; }
}
