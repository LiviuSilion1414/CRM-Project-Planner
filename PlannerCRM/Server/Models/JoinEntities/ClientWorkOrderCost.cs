namespace PlannerCRM.Server.Models.JoinEntities;

public class ClientWorkOrderCost
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClient FirmClient { get; set; }
    public WorkOrderCost WorkOrderCost { get; set; }
}
