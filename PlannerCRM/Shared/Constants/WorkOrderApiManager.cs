namespace PlannerCRM.Client.Models;

public static class WorkOrderApiManager
{
    public static string SearchWorkOrderByTitle(string worOrderTitle)
        => $"searchWorkOrderByTitle/{worOrderTitle}";

    public static string FindAssociatedActivitiesByWorkOrderId(int workOrderId)
        => $"findAssociatedActivitiesByWorkOrderId/{workOrderId}";
    
    public static string FindAssociatedWorkOrdersByClientId(int clientId)
        => $"findAssociatedWorkOrdersByClientId/{clientId}";
}