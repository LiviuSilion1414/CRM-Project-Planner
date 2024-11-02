namespace PlannerCRM.Server.Models.Entities;

public class FirmClient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    // Navigation properties
    public ICollection<WorkOrder> WorkOrders { get; set; }
    public ICollection<WorkOrderCost> WorkOrderCosts { get; set; }
}
