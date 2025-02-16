using System.Security.Claims;

namespace PlannerCRM.Shared.Models;

public class CurrentUser
{
    public Guid  Guid { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Token { get; set; }
    public bool IsAuthenticated { get; set; }
    public List<string> Roles { get; set; }
    public Dictionary<string, string> Claims { get; set; }
    public List<Claim> ClaimsOk { get; set; }
}