namespace PlannerCRM.Server.Models.JoinEntities;

public class ClientWorkOrderCost
{
    public Guid Guid { get; set; }
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClient FirmClient { get; set; }
    public WorkOrderCost WorkOrderCost { get; set; }
}
