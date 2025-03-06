namespace PlannerCRM.Shared.ApiHelpers.Endpoints;

public class EmployeeEndpointActions
{
    #region Endpoints
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
    #endregion

    #region Controller endpoints routes
    public const string ADD_PLACEHOLDER = "add";
    public const string EDIT_PLACEHOLDER = "edit";
    public const string DELETE_PLACEHOLDER = "delete";
    public const string GET_BY_ID_PLACEHOLDER = "getById";
    public const string GET_WITH_PAGINATION_PLACEHOLDER = "getWithPagination";

    public const string ASSIGN_ROLE_PLACEHOLDER = "assignRole";
    public const string SEARCH_EMPLOYEE_BY_NAME_PLACEHOLDER = "searchEmployeeByName";
    public const string SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_PLACEHOLDER = "searchEmployeeByNameForRecovery";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_PLACEHOLDER = "findAssociatedActivitiesByEmployeeId";
    #endregion
}