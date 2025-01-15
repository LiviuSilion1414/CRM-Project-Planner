namespace PlannerCRM.Client.Models;

public static class ClientApiManager
{
    public static string ActionUrl { get; private set; }

    public static void SearchClientByName(string clientName)
        => ActionUrl = $"searchClientByName/{clientName}";

    public static void FindAssociatedWorkOrdersByClientId(int clientId)
        => ActionUrl = $"findAssociatedWorkOrdersByClientId/{clientId}";
}