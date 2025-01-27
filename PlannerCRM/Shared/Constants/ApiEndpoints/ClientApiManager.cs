namespace PlannerCRM.Shared.Constants.ApiEndpoints;

public static class ClientApiManager
{
    public static string SearchClientByName(string clientName)
        => $"searchClientByName/{clientName}";

    public static string FindAssociatedWorkOrdersByClientId(int clientId)
        => $"findAssociatedWorkOrdersByClientId/{clientId}";
}