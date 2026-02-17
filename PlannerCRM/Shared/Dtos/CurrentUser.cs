using System.Security.Claims;

namespace PlannerCRM.Shared.Dtos;

public class CurrentUserDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string token { get; set; }
    public bool isAuthenticated { get; set; }
    public List<Guid>? menuRolesList { get; set; } 
    public List<Claim> claims { get; set; }
}