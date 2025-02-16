namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class EmployeeEndpointActions
{
    public const string ASSIGN_ROLE = "assignRole/{roleName}";
    public const string SEARCH_EMPLOYEE_BY_NAME = "searchEmployeeByName/{title}";
    public const string SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY = "searchEmployeeByNameForRecovery/{name}/{email}/{phone}";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID = "findAssociatedActivitiesByEmployeeId/{itemId}";
    public const string FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID = "findAssociatedWorkTimesByActivityIdAndEmployeeId/{itemId}/{item2}";
    public const string FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID ="findAssociatedSalaryDataByEmployeeId/{itemId}";

    public static string SearchEmployeeByName(string query) =>$"searchEmployeeByName/{query}";
    public static string AssignRole(string roleName) => $"searchEmployeeByName/{roleName}";

    public static string SearchEmployeeByName(string name, string email = "", string phone = "") => $"searchEmployeeByNameForRecovery/{name}/{email}/{phone}";
    public static string FindAssociatedActivitiesByEmployeeId(string employeeId) =>$"findAssociatedActivitiesByEmployeeId/{employeeId}";
    public static string FindAssociatedWorktimesByActivityIdAndEmployeeid(string employeeId, string activityId) =>$"findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId}/{activityId}";
    public static string FindAssociatedSalaryDataByEmployeeId(string employeeId) =>$"findAssociatedSalaryDataByEmployeeId/{employeeId}";
}