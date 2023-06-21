using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.Workorder.Forms;

public partial class WorkOrderAddFormDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = """ Campo "Nome" richiesto. """)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = """ Campo "Data d'inizio" richiesto. """)]
    [WorkOrderStartDateRange(ErrorMessage = "Il periodo contrattuale dev'essere tra 3 e 24 mesi.")]
    public DateTime? StartDate { get; set; }
    
    [Required(ErrorMessage = """ Campo "Data di fine" richiesto. """)]
    [WorkOrderFinishDateRange(MIN_WORKORDER_MONTH_CONTRACT, MAX_WORKORDER_MONTH_CONTRACT, 
        ErrorMessage = "Il periodo contrattuale dev'essere tra 3 e 24 mesi.")]
    public DateTime? FinishDate { get; set; }
}