namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class EmployeeEndpointActions
{
    public const string ADD_BASE = "employee/add";
    public const string EDIT_BASE = "employee/edit";
    public const string DELETE_BASE = "employee/delete";
    public const string GET_BY_ID_BASE = "employee/getById";
    public const string GET_WITH_PAGINATION_BASE = "employee/getWithPagination";

    public const string ASSIGN_ROLE_BASE = "employee/assignRole";
    public const string SEARCH_EMPLOYEE_BY_NAME_BASE = "employee/searchEmployeeByName";
    public const string SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_BASE = "employee/searchEmployeeByNameForRecovery";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_BASE = "employee/findAssociatedActivitiesByEmployeeId";
    public const string FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID_BASE = "employee/findAssociatedWorkTimesByActivityIdAndEmployeeId";
    public const string FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID_BASE = "employee/findAssociatedSalaryDataByEmployeeId";

    public const string ASSIGN_ROLE_PLACEHOLDER = "employee/assignRole/{roleName}";
    public const string SEARCH_EMPLOYEE_BY_NAME_PLACEHOLDER = "employee/searchEmployeeByName/{title}";
    public const string SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_PLACEHOLDER = "employee/searchEmployeeByNameForRecovery/{name}/{email}/{phone}";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_PLACEHOLDER = "employee/findAssociatedActivitiesByEmployeeId/{itemId}";
    public const string FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID_PLACEHOLDER = "employee/findAssociatedWorkTimesByActivityIdAndEmployeeId/{itemId}/{item2}";
    public const string FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID_PLACEHOLDER = "employee/findAssociatedSalaryDataByEmployeeId/{itemId}";

    public static string Add() => ADD_BASE;
    public static string Edit() => EDIT_BASE;
    public static string Delete() => DELETE_BASE;
    public static string Get(Guid guid) => $"{GET_BY_ID_BASE}/{guid}";
    public static string GetWithPagination(int limit, int offset) => $"{GET_WITH_PAGINATION_BASE}/{limit}/{offset}";

    public static string SearchEmployeeByName(string query) =>$"{SEARCH_EMPLOYEE_BY_NAME_BASE}/{query}";
    public static string AssignRole(string roleName) => $"{ASSIGN_ROLE_BASE}/{roleName}";
    public static string SearchEmployeeByName(string name, string email = "", string phone = "") => $"{SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_BASE}/{name}/{email}/{phone}";
    public static string FindAssociatedActivitiesByEmployeeId(string employeeId) =>$"{FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_BASE}/{employeeId}";
    public static string FindAssociatedWorktimesByActivityIdAndEmployeeId(string employeeId, string activityId) =>$"{FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID_BASE}/{employeeId}/{activityId}";
    public static string FindAssociatedSalaryDataByEmployeeId(string employeeId) =>$"{FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID_BASE}/{employeeId}";
}