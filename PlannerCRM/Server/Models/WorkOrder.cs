namespace PlannerCRM.Server.Models;

public class WorkOrder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public List<Activity> Activities { get; set; }
    public List<WorkTimeRecord> WorkTimeRecords { get; set; }
}


