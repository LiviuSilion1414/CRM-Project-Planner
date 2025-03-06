namespace PlannerCRM.Server.Models.Entities;

public class FirmClient
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    // Navigation properties
    public List<WorkOrder> WorkOrders { get; set; }
}
