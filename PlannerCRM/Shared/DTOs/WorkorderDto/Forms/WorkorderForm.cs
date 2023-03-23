using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs.Workorder.Forms;

public partial class WorkorderForm
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime FinishDate { get; set; }
}