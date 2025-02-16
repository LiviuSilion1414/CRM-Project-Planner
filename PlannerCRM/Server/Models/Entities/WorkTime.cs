namespace PlannerCRM.Server.Models.Entities;

public class WorkTime
{
    public Guid Guid { get; set; }
    public DateTime CreationDate { get; set; }

    public double WorkedHours { get; set; }

    // Navigation properties
    public Guid WorkOrderId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ActivityId { get; set; }
    public WorkOrder WorkOrder { get; set; }
    public Employee Employee { get; set; }
    public Activity Activity { get; set; }
    public List<ActivityWorkTime> ActivityWorkTimes { get; set; }
}
