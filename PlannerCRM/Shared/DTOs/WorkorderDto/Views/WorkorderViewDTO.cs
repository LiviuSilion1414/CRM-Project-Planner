using PlannerCRM.Shared.DTOs.ClientDto;

namespace PlannerCRM.Shared.DTOs.WorkOrder.Views;

public class WorkOrderViewDto
{
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool IsInvoiceCreated { get; init; }
    public ClientViewDto Client {  get; set; }
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsArchived { get; set; }
}