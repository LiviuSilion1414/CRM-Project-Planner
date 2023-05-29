using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.DTOs.Workorder.Forms;

public partial class WorkOrderEditFormDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime FinishDate { get; set; }
}