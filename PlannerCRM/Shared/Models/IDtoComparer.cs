namespace PlannerCRM.Shared.Models;

public interface IDtoComparer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsArchived { get; set; }
}