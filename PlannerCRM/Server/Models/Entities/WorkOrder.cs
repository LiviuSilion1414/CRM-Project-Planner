namespace PlannerCRM.Server.Models.Entities;

public class WorkOrder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Navigation properties
    public int FirmClientId { get; set; }
    public int WorkOrderCostId { get; set; }
    public FirmClient FirmClient { get; set; }
    public WorkOrderCost WorkOrderCost { get; set; }
    public List<Activity> Activities { get; set; }
}
