namespace PlannerCRM.Server.Models;

public class ClientWorkOrder
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    public int WorkOrderId { get; set; }
}