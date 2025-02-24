namespace PlannerCRM.Shared.Models;

public class SearchFilterDto
{
    public Guid Guid { get; set; }
    public Guid Guid2 { get; set; }

    public int Limit { get; set; }
    public int Offset { get; set; }
    public int PageSize { get; set; }
    public string SearchQuery { get; set; }
    public List<string> Roles { get; set; }
    public object Data { get; set; }
}
