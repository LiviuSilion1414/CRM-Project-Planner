namespace PlannerCRM.Server.Models;

public class WorkOrderInvoice 
{
    public int Id { get; set; }
    public DateTime IssuedDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int WorkOrderId { get; set; }
    public int ClientId { get; set; }
}