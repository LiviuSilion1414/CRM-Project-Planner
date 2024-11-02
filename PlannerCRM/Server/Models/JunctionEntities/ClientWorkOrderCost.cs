namespace PlannerCRM.Server.Models.JunctionEntities;

using PlannerCRM.Server.Models.Entities;

public class ClientWorkOrderCost
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int WorkOrderCostId { get; set; }

    // Navigation properties
    public Client Client { get; set; }
    public WorkOrderCost WorkOrderCost { get; set; }
}
