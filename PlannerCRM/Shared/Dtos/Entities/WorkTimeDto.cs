namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkTimeDto
{
    public int Id { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string CreationDateString { get => string.Format("{0:dd/MM/yyyy}", CreationDate); }

    public double WorkedHours { get; set; }
    public int WorkOrderId { get; set; }
    public int EmployeeId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    //public WorkOrderDto WorkOrder { get; set; }
    //public EmployeeDto Employee { get; set; }
    //public ActivityDto Activity { get; set; }
    //public List<ActivityWorkTimeDto> ActivityWorkTimes { get; set; }
}
