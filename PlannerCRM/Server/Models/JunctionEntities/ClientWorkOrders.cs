namespace PlannerCRM.Server.Models.JunctionEntities;

using PlannerCRM.Server.Models.Entities;

public class ClientWorkOrder
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderId { get; set; }

    // Navigation properties
    public FirmClient FirmClient { get; set; }
    public WorkOrder WorkOrder { get; set; }
}
