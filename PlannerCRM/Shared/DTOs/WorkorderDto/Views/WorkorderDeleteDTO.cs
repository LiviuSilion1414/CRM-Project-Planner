namespace PlannerCRM.Shared.DTOs.Workorder.Views;

public class WorkOrderDeleteDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime FinishDate { get; set; }
}