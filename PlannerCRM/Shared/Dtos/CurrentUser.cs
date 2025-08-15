using System.Security.Claims;

namespace PlannerCRM.Shared.Dtos;

public class CurrentUserDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string token { get; set; }
    public bool isAuthenticated { get; set; }
    public List<string> roles { get; set; } = new List<string>();

    public List<Claim> claims { get; set; }
}