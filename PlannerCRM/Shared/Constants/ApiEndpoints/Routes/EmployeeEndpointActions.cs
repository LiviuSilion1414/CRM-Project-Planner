namespace PlannerCRM.Shared.Constants.ApiEndpoints.Routes;

public class EmployeeEndpointActions
{
    public const string SEARCH_EMPLOYEE_BY_NAME = 
        $"searchEmployeeByName/{EndpointsPlaceholders.SEARCH_QUERY}";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID = 
        $"findAssociatedActivitiesByEmployeeId/{EndpointsPlaceholders.ITEM_ID_1}";
    public const string FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID = 
        $"findAssociatedWorkTimesByActivityIdAndEmployeeId/{EndpointsPlaceholders.ITEM_ID_1}/{EndpointsPlaceholders.ITEM_ID_2}";
    public const string FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID = 
        $"findAssociatedSalaryDataByEmployeeId/{EndpointsPlaceholders.ITEM_ID_1}";

    public static string SearchEmployeeByName(string query) =>
        $"searchEmployeeByName/{query}";
    public static string FindAssociatedActivitiesByEmployeeId(string employeeId) =>
        $"findAssociatedActivitiesByEmployeeId/{employeeId}";
    public static string FindAssociatedWorktimesByActivityIdAndEmployeeid(string employeeId, string activityId) =>
        $"findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId}/{activityId}";
    public static string FindAssociatedSalaryDataByEmployeeId(string employeeId) =>
        $"findAssociatedSalaryDataByEmployeeId/{employeeId}";
}