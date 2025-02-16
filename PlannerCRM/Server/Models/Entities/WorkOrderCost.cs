namespace PlannerCRM.Server.Models.Entities;

public class WorkOrderCost
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public decimal TotalCost { get; set; }

    // Navigation properties
    public Guid WorkOrderId { get; set; }
    public Guid FirmClientId { get; set; }
    public WorkOrder WorkOrder { get; set; }
    public FirmClient FirmClient { get; set; }
}
