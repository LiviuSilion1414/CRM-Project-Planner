using PlannerCRM.Server.Models.JunctionEntities;
using System.Diagnostics;

namespace PlannerCRM.Server.Models.Entities;

public class WorkTime
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public int WorkOrderId { get; set; }
    public int EmployeeId { get; set; }
    public double WorkedHours { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; }
    public Employee Employee { get; set; }
    public Activity Activity { get; set; }
    public ICollection<ActivityWorkTime> ActivityWorkTimes { get; set; }
}
