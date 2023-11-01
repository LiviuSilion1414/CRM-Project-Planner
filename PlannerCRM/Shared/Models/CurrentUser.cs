namespace PlannerCRM.Shared.Models;

public class CurrentUser
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public bool IsAuthenticated { get; set; }
    public Dictionary<string, string> Claims { get; set; }
}