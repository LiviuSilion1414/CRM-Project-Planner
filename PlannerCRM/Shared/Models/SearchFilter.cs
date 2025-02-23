namespace PlannerCRM.Shared.Models;

public class SearchFilter
{
    public Guid Guid { get; set; }

    public int Limit { get; set; }
    public int Offset { get; set; }
    public int PageSize { get; set; }

    public object Data { get; set; }
}
