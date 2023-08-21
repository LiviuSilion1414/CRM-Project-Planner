namespace PlannerCRM.Shared.Dtos.ClientDto;

public class ClientFormDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = """ Campo "Nome" richiesto """)]
    public string Name { get; set; }

    [Required(ErrorMessage = """ Campo "Partita IVA" richiesto """)]
    public string VatNumber { get; set; }

    public int WorkOrderId { get; set; }
}