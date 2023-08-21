namespace PlannerCRM.Shared.Dtos.ClientDto;

public class ClientFormDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    public int WorkOrderId { get; set; }
}