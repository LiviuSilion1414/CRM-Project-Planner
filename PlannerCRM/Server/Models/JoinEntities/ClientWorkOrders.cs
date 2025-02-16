using System.ComponentModel.DataAnnotations.Schema;

namespace PlannerCRM.Server.Models.JoinEntities;

public class ClientWorkOrder
{
    public Guid Guid { get; set; }
    public Guid FirmClientId { get; set; }
    public Guid WorkOrderId { get; set; }

    // Navigation properties
    [NotMapped]
    public FirmClient FirmClient { get; set; }
    [NotMapped]
    public WorkOrder WorkOrder { get; set; }
}
