namespace PlannerCRM.Client.Pages.Developer.Senior;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public partial class SeniorDeveloper : ComponentBase
{
    [Parameter] public int EmployeeId { get; set; }
}