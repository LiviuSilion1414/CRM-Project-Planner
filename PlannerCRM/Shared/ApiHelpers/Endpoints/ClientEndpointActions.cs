namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class ClientEndpointActions
{
    public const string ADD_BASE = "client/add";
    public const string EDIT_BASE = "client/edit";
    public const string DELETE_BASE = "client/delete";
    public const string GET_BY_ID_BASE = "client/getById";
    public const string GET_WITH_PAGINATION_BASE = "client/getWithPagination";

    public const string SEARCH_CLIENT_BY_NAME_BASE = "client/searchClientByName";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_BASE = "client/findAssociatedWorkOrdersByClientId";

    public const string SEARCH_CLIENT_BY_NAME_PLACEHOLDER = "client/searchClientByName/{title}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER = "client/findAssociatedWorkOrdersByClientId/{clientId}";

    public string Add() => ADD_BASE;
    public string Edit() => EDIT_BASE;
    public string Delete() => DELETE_BASE;
    public string Get(Guid guid) => $"{GET_BY_ID_BASE}/{guid}";
    public string GetWithPagination(int limit, int offset) => $"{GET_WITH_PAGINATION_BASE}/{limit}/{offset}";

    public static string SearchClientByName(string query) => $"{SEARCH_CLIENT_BY_NAME_BASE}/{query}";
    public static string FindAssociatedWorkOrdersByClientId(Guid clientId) => $"{FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_BASE}/{clientId}";
}