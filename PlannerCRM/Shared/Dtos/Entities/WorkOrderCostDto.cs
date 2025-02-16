namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderCostDto
{
    public Guid  Guid { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string CreationDateString { get => string.Format("{0:dd/MM/yyyy}", CreationDate); }
    public decimal TotalCost { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid FirmClientId { get; set; }

    // Navigation properties
    //public WorkOrderDto WorkOrder { get; set; }
    //public FirmClientDto FirmClient { get; set; }
}
