namespace PlannerCRM.Shared.DTOs.CostDto;

public class WorkOrderInvoiceDto
{
    public int Id { get; init; }
    public DateTime IssuedDate { get; init; }
    public decimal TotalAmount { get; init; }
    public int WorkOrderId { get; init; }
    public int ClientId { get; init; }
}