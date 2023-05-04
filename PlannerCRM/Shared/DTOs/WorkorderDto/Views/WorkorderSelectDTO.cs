using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs.Workorder.Views;

public class WorkorderSelectDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}