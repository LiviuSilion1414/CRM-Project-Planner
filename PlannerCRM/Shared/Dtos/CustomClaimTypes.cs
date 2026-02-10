using System.Security.Claims;

namespace PlannerCRM.Shared.Dtos;

public class CustomClaimTypes
{
    public const string Guid = "id";
    public const string Name = "title";
    public const string Email = "username";
    public const string Role = "role";
    public const string Menu = "menu";
    public const string IsAuthenticated = "isAuthenticated";
    public const string Token = "token";
}
