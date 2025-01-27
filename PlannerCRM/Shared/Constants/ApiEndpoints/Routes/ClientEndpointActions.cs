namespace PlannerCRM.Shared.Constants.ApiEndpoints.Routes;

public class ClientEndpointActions
{
    public const string SEARCH_CLIENT_BY_NAME = "searchClientByName/{title}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID = "findAssociatedWorkOrdersByClientId/{item1}";

    public static string SearchClientByName(string query) => $"searchClientByName/{query}";
    public static string FindAssociatedWorkOrdersByClientId(int clientId) => $"findAssociatedWorkOrdersByClientId/{clientId}";
}