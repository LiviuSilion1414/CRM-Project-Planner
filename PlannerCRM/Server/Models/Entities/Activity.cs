namespace PlannerCRM.Server.Models.Entities;

public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int WorkOrderId { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
