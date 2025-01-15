namespace PlannerCRM.Client.Models;

public static class WorkOrderApiManager
{
    public static string ActionUrl { get; private set; }

    public static void SearchWorkOrderByTitle(string worOrderTitle)
        => ActionUrl = $"searchWorkOrderByTitle/{worOrderTitle}";

    public static void FindAssociatedActivitiesByWorkOrderId(int workOrderId)
        => ActionUrl = $"findAssociatedActivitiesByWorkOrderId/{workOrderId}";
    
    public static void FindAssociatedWorkOrdersByClientId(int clientId)
        => ActionUrl = $"findAssociatedWorkOrdersByClientId/{clientId}";
}