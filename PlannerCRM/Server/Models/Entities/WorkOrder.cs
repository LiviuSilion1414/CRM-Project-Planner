namespace PlannerCRM.Server.Models.Entities;

public class WorkOrder
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Navigation properties
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderCostId { get; set; }
    public FirmClient FirmClient { get; set; }
    public List<Activity> Activities { get; set; }
}
