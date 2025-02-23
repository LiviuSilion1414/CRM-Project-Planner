namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class ActivityEndpointActions
{
    public const string ADD_BASE = "activity/add";
    public const string EDIT_BASE = "activity/edit";
    public const string DELETE_BASE = "activity/delete";
    public const string GET_BY_ID_BASE = "activity/getById";
    public const string GET_WITH_PAGINATION_BASE = "activity/getWithPagination";

    private const string SEARCH_BY_TITLE_BASE = "activity/searchByTitle";
    private const string FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_BASE = "activity/findAssociatedEmployeesByActivityId";
    private const string FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_BASE = "activity/findAssociatedWorkOrdersByActivityId";
    private const string FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_BASE = "activity/findAssociatedWorkTimesWithinActivity";

    public const string SEARCH_BY_TITLE_PLACEHOLDER = "searchByTitle/{title}";
    public const string FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_PLACEHOLDER = "findAssociatedEmployeesByActivityId/{itemId}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_PLACEHOLDER = "findAssociatedWorkOrdersByActivityId/{itemId}";
    public const string FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_PLACEHOLDER = "findAssociatedWorkTimesWithinActivity/{itemId}";

    public string Add() => ADD_BASE;
    public string Edit() => EDIT_BASE;
    public string Delete() => DELETE_BASE;
    public string Get(Guid guid) => $"{GET_BY_ID_BASE}/{guid}";
    public string GetWithPagination(int limit, int offset) => $"{GET_WITH_PAGINATION_BASE}/{limit}/{offset}";

    public static string SearchByTitle(string query) => $"{SEARCH_BY_TITLE_BASE}/{query}";
    public static string FindAssociatedEmployeesByActivityId(int activityId) => $"{FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_BASE}/{activityId}";
    public static string FindAssociatedWorkOrdersByActivityId(int activityId) => $"{FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_BASE}/{activityId}";
    public static string FindAssociatedWorktimesByActivityId(int activityId) => $"{FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_BASE}/{activityId}";
}