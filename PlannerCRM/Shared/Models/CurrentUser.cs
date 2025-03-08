using System.Security.Claims;

namespace PlannerCRM.Shared.Models;

public class CurrentUser
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string? token { get; set; }
    public bool isAuthenticated { get; set; }
    public List<string> roles { get; set; }

    public List<Claim> claims { get; set; }
}