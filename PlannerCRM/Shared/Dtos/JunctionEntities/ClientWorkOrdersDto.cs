using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;

public class ClientWorkOrderDto
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClient { get; set; }
    public WorkOrderDto WorkOrder { get; set; }
}
