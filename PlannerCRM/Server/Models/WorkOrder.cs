namespace PlannerCRM.Server.Models;

public class WorkOrder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool IsInvoiceCreated { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public int ClientId { get; set; }
    
    [NotMapped]
    public List<Activity> Activities { get; set; }

    [NotMapped]
    public List<WorkTimeRecord> WorkTimeRecords { get; set; }

    [NotMapped]
    public FirmClient Client { get; set; }
}


