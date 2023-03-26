using PlannerCRM.Shared.DTOs.ActivityDto.Views;

namespace PlannerCRM.Shared.DTOs.Workorder.Views;

public class WorkorderViewDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
   // public List<ActivityViewDTO> Activities { get; set; }
}