namespace PlannerCRM.Shared.Dtos.Entities;

public class WorkOrderDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int FirmClientId { get; set; }
    public int WorkOrderCostId { get; set; }

    // Navigation properties
    public FirmClientDto FirmClientDto { get; set; }
    public WorkOrderCostDto WorkOrderCostDto { get; set; }
    public ICollection<ActivityDto> ActivitiesDto { get; set; }
}
