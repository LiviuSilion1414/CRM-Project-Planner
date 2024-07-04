namespace PlannerCRM.Server.Models;

public class FirmClient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    [NotMapped]
    public List<ClientWorkOrder> WorkOrders { get; set; }
}