namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class ClientWorkOrderDto
{
    public Guid  Guid { get; set; }
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClient { get; set; }
    public WorkOrderDto WorkOrder { get; set; }
}
