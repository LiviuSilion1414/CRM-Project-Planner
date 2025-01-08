namespace PlannerCRM.Client.Models;

public class CascadingDataContainer
{
    public bool IsSelectSelected { get; set; }
    public bool IsAddSelected { get; set; }
    public bool IsUpdateSelected { get; set; }
    public bool IsDeleteSingleItemSelected { get; set; }
    public bool IsDeleteMultipleSelected { get; set; }

    public bool IsOperationDone { get; set; }
}
