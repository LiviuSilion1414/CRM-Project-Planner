namespace PlannerCRM.Client.Models;

public static class EmployeeApiManager
{
    public static string SearchEmployeeByName(string employeeName)
        => $"searchEmployeeByName/{employeeName}";

    public static string FindAssociatedActivitiesByEmployeeId(int employeeId)
        => $"findAssociatedActivitiesByEmployeeId/{employeeId}";

    public static string FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
        => $"findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId}/{activityId}";

    public static string FindAssociatedSalaryDataByEmployeeId(int employeeId)
        => $"findAssociatedSalaryDataByEmployeeId/{employeeId}";
}