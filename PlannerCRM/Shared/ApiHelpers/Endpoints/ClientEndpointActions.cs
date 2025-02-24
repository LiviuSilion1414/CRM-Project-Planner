namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class ClientEndpointActions
{
    #region Endpoints
    public const string ADD_BASE = "client/add";
    public const string EDIT_BASE = "client/edit";
    public const string DELETE_BASE = "client/delete";
    public const string GET_BY_ID_BASE = "client/getById";
    public const string GET_WITH_PAGINATION_BASE = "client/getWithPagination";
    public const string SEARCH_CLIENT_BY_NAME_BASE = "client/searchClientByName";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_BASE = "client/findAssociatedWorkOrdersByClientId";
    #endregion

    #region Controller routes endpoints
    public const string ADD_PLACEHOLDER = "add";
    public const string EDIT_PLACEHOLDER = "edit";
    public const string DELETE_PLACEHOLDER = "delete";
    public const string GET_BY_ID_PLACEHOLDER = "getById";
    public const string GET_WITH_PAGINATION_PLACEHOLDER = "getWithPagination";

    public const string SEARCH_CLIENT_BY_NAME_PLACEHOLDER = "searchClientByName";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER = "findAssociatedWorkOrdersByClientId";
    #endregion
}