namespace PlannerCRM.Server.Models.Entities;

public class WorkOrder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [NotMapped]
    public int FirmClientId { get; set; }

    [NotMapped]
    public int WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClient FirmClient { get; set; }
    public WorkOrderCost WorkOrderCost { get; set; }
    public ICollection<Activity> Activities { get; set; }
}
