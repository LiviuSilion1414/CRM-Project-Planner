namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class WorkOrderEndpointActions
{
    public const string ADD_BASE = "workOrder/add";
    public const string EDIT_BASE = "workOrder/edit";
    public const string DELETE_BASE = "workOrder/delete";
    public const string GET_BY_ID_BASE = "workOrder/getById";
    public const string GET_WITH_PAGINATION_BASE = "workOrder/getWithPagination";
    public const string SEARCH_WORKORDER_BY_TITLE_BASE = "workOrder/searchWorkOrderByTitle";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_BASE = "workOrder/findAssociatedActivitiesByWorkOrderId";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_BASE = "workOrder/findAssociatedWorkOrdersByClientId";

    public const string SEARCH_WORKORDER_BY_TITLE_PLACEHOLDER = "searchWorkOrderByTitle/{title}";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_PLACEHOLDER = "findAssociatedActivitiesByWorkOrderId/{itemId}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER = "findAssociatedWorkOrdersByClientId/{itemId}";

    public static string Add() => ADD_BASE;
    public static string Edit() => EDIT_BASE;
    public static string Delete() => DELETE_BASE;
    public static string Get(Guid guid) => $"{GET_BY_ID_BASE}/{guid}";
    public static string GetWithPagination(int limit, int offset) => $"{GET_WITH_PAGINATION_BASE}/{limit}/{offset}";

    public static string SearchWorkOrderByTitle(string query) => $"{SEARCH_WORKORDER_BY_TITLE_BASE}/{query}";
    public static string FindAssociatedActivitiesByWorkOrderId(Guid workorderid) => $"{FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_BASE}/{workorderid}";
    public static string FindAssociatedWorkordersByClientId(Guid workorderid) => $"{FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_BASE}/{workorderid}";
}