namespace PlannerCRM.Shared.Constants.ApiEndpoints.Routes;

public class ActivityEndpointActions
{
    public const string SEARCH_BY_TITLE = 
        $"searchByTitle/{EndpointsPlaceholders.SEARCH_QUERY}";
    public const string FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID = 
        $"findAssociatedEmployeesByActivityId/{EndpointsPlaceholders.ITEM_ID_1}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID = 
        $"findAssociatedWorkOrdersByActivityId/{EndpointsPlaceholders.ITEM_ID_1}";
    public const string FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY = 
        $"findAssociatedWorkTimesWithinActivity/{EndpointsPlaceholders.ITEM_ID_1}";

    public static string SearchByTitle(string query) => $"searchByTitle/{query}";
    public static string FindAssociatedEmployeesByActivityid(int activityId) => 
        $"findAssociatedEmployeesByActivityId/{activityId}";
    public static string FindAssociatedworkordersByActivityId(int activityId) => 
        $"findAssociatedWorkOrdersByActivityId/{activityId}";
    public static string FindAssociatedWorktimesByActivityId(int activityId) => 
        $"findAssociatedWorkTimesWithinActivity/{activityId}";
}