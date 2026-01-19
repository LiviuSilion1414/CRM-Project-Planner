namespace PlannerCRM.Shared.Dtos;

public partial class FilterDto
{
    public Guid? id { get; set; }
    public string? searchQuery { get; set; }
    public object? data { get; set; }
}
