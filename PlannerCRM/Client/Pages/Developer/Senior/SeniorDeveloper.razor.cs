using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.Developer.Senior;


[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public partial class SeniorDeveloper
{
    [Parameter] public int EmployeeId { get; set; }
}