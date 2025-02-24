namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class WorkOrderEndpointActions
{
    #region Endpoints
    public const string ADD_BASE = "workOrder/add";
    public const string EDIT_BASE = "workOrder/edit";
    public const string DELETE_BASE = "workOrder/delete";
    public const string GET_BY_ID_BASE = "workOrder/getById";
    public const string GET_WITH_PAGINATION_BASE = "workOrder/getWithPagination";

    public const string SEARCH_WORKORDER_BY_TITLE_BASE = "workOrder/searchWorkOrderByTitle";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_BASE = "workOrder/findAssociatedActivitiesByWorkOrderId";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_BASE = "workOrder/findAssociatedWorkOrdersByClientId";
    #endregion

    #region Controller endpoints routes
    public const string ADD_PLACEHOLDER = "add";
    public const string EDIT_PLACEHOLDER = "edit";
    public const string DELETE_PLACEHOLDER = "delete";
    public const string GET_BY_ID_PLACEHOLDER = "getById";
    public const string GET_WITH_PAGINATION_PLACEHOLDER = "getWithPagination";

    public const string SEARCH_WORKORDER_BY_TITLE_PLACEHOLDER = "searchWorkOrderByTitle/{title}";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_PLACEHOLDER = "findAssociatedActivitiesByWorkOrderId/{itemId}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER = "findAssociatedWorkOrdersByClientId/{itemId}";
    #endregion
}