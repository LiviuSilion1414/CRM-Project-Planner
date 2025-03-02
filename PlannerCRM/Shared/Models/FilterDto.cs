namespace PlannerCRM.Shared.Models;

public partial class FilterDto
{
    public Guid Id { get; set; }
    public string SearchQuery { get; set; }
    public object Data { get; set; }
}
