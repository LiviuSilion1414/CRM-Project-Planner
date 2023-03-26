namespace PlannerCRM.Shared.DTOs.ActivityDto.Views;

public class ActivityDeleteDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public string WorkorderActivity { get; set; }
}