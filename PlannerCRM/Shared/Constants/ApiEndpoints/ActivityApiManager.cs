namespace PlannerCRM.Shared.Constants.ApiEndpoints;

public static class ActivityApiManager
{
    public static string ActionUrl { get; private set; }

    public static void SearchByTitle(string activityTitle)
        => ActionUrl = $"searchByTitle/{activityTitle}";

    public static void FindAssociatedEmployeesByActivityId(int activityId)
        => ActionUrl = $"findAssociatedEmployeesByActivityId/{activityId}";

    public static void FindAssociatedWorkOrdersByActivityId(int activityId)
        => ActionUrl = $"findAssociatedWorkOrdersByActivityId/{activityId}";

    public static void FindAssociatedWorkTimesWithinActivity(int activityId)
        => ActionUrl = $"findAssociatedWorkTimesWithinActivity/{activityId}";
}