namespace PlannerCRM.Server.Models.Entities;

public class WorkOrderCost
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public decimal TotalCost { get; set; }

    [NotMapped]
    public int WorkOrderId { get; set; }
    
    [NotMapped]
    public int FirmClientId { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; }
    public FirmClient FirmClient { get; set; }
}
