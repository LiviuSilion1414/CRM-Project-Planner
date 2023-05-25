using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.Developer.Junior;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class JuniorDeveloper
{
    [Parameter] public int EmployeeId { get; set; }
}