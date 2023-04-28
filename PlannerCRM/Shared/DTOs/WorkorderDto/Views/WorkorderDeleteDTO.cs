namespace PlannerCRM.Shared.DTOs.Workorder.Views;

public class WorkorderDeleteDTO
{
	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime FinishDate { get; set; }
}