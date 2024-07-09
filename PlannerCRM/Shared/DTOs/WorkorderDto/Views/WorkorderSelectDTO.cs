using PlannerCRM.Shared.DTOs.ClientDto;

namespace PlannerCRM.Shared.DTOs.WorkOrder.Views;

public class WorkOrderSelectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ClientViewDto Client { get; set; }
}