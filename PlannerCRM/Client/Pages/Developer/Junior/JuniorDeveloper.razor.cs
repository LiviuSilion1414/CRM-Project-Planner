namespace PlannerCRM.Client.Pages.Developer.Junior;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class JuniorDeveloper : ComponentBase
{
    [Parameter] public string EmployeeId { get; set; }
}