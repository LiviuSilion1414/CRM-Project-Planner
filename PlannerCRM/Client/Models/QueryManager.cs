namespace PlannerCRM.Client.Models;

public class QueryManager
{
    public string Query { get; set; } = string.Empty;

    public bool HasQuery => !string.IsNullOrEmpty(Query);
}
