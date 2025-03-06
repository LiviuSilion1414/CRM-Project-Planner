namespace PlannerCRM.Server.Models.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Navigation properties
    public Guid WorkOrderId { get; set; }
    public WorkOrder WorkOrder { get; set; }
    public List<Employee> Employees { get; set; }
    public List<EmployeeActivity> EmployeeActivities { get; set; }
}