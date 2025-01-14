namespace PlannerCRM.Client.Models;

public class ComponentMetadata
{
    public Type ComponentType { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}