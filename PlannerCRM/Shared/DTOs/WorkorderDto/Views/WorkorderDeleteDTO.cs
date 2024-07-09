using PlannerCRM.Shared.DTOs.ClientDto;

namespace PlannerCRM.Shared.DTOs.WorkOrder.Views;

public class WorkOrderDeleteDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime FinishDate { get; set; }
	public ClientViewDto Client { get; set; }
}