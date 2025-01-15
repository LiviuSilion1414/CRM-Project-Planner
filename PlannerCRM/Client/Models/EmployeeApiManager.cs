namespace PlannerCRM.Client.Models;

public static class EmployeeApiManager
{
    public static string ActionUrl { get; private set; }

    public static void SearchEmployeeByName(string employeeName)
        => ActionUrl = $"searchEmployeeByName/{employeeName}";

    public static void FindAssociatedActivitiesByEmployeeId(int employeeId)
        => ActionUrl = $"findAssociatedActivitiesByEmployeeId/{employeeId}";

    public static void FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
        => ActionUrl = $"findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId}/{activityId}";

    public static void FindAssociatedSalaryDataByEmployeeId(int employeeId)
        => ActionUrl = $"findAssociatedSalaryDataByEmployeeId/{employeeId}";
}