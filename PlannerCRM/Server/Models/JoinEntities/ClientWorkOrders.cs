using System.ComponentModel.DataAnnotations.Schema;

namespace PlannerCRM.Server.Models.JoinEntities;

public class ClientWorkOrder
{
    public int Id { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderId { get; set; }

    // Navigation properties
    [NotMapped]
    public FirmClient FirmClient { get; set; }
    [NotMapped]
    public WorkOrder WorkOrder { get; set; }
}
