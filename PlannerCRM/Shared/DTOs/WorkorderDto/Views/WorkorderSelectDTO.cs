namespace PlannerCRM.Shared.DTOs.Workorder.Views;

public class WorkOrderSelectDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}