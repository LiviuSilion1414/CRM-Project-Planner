using PlannerCRM.Shared.DTOs.ClientDto;

namespace PlannerCRM.Shared.DTOs.Workorder.Views;

public class WorkOrderViewDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsCompleted { get; set; }
    public ClientViewDto Client { get; set; }
}