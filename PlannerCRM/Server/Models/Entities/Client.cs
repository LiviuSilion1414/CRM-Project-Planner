namespace PlannerCRM.Server.Models.Entities;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    // Navigation properties
    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
    public ICollection<WorkOrderCost> WorkOrderCosts { get; set; } = new List<WorkOrderCost>();
}
