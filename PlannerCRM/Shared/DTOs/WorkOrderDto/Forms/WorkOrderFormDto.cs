namespace PlannerCRM.Shared.DTOs.Workorder.Forms;

public partial class WorkOrderFormDto
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

    [IsNotZero(ErrorMessage = """ Cliente non riconosciuto. """)]
    [Required(ErrorMessage = """ Campo "Cliente" richiesto """)]
    public int ClientId { get; set; }

    public string ClientName { get; set; }
}