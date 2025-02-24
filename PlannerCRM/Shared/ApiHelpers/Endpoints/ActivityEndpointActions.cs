namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class ActivityEndpointActions
{
    #region Endpoints
    public const string SEARCH_BY_TITLE_BASE = "activity/searchByTitle";
    public const string FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_BASE = "activity/findAssociatedEmployeesByActivityId";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_BASE = "activity/findAssociatedWorkOrdersByActivityId";
    public const string FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_BASE = "activity/findAssociatedWorkTimesWithinActivity";

    public const string ADD_BASE = "activity/add";
    public const string EDIT_BASE = "activity/edit";
    public const string DELETE_BASE = "activity/delete";
    public const string GET_BY_ID_BASE = "activity/getById";
    public const string GET_WITH_PAGINATION_BASE = "activity/getWithPagination";
    #endregion

    #region Controllers route endpoints
    public const string ADD_PLACEHOLDER = "add";
    public const string EDIT_PLACEHOLDER = "edit";
    public const string DELETE_PLACEHOLDER = "delete";
    public const string GET_BY_ID_PLACEHOLDER = "getById";
    public const string GET_WITH_PAGINATION_PLACEHOLDER = "getWithPagination";

    public const string SEARCH_BY_TITLE_PLACEHOLDER = "searchByTitle";
    public const string FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_PLACEHOLDER = "findAssociatedEmployeesByActivityId";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_PLACEHOLDER = "findAssociatedWorkOrdersByActivityId";
    public const string FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_PLACEHOLDER = "findAssociatedWorkTimesWithinActivity";
    #endregion
}