namespace PlannerCRM.Client.Models;

public class ControllersNamesHelper
{
    private const string EMPLOYEE = "employee";
    private const string FIRM_CLIENT = "client";
    private const string WORK_ORDER = "workOrder";
    private const string ACTIVITY = "activity";
    private const string WORK_TIME = "workTime";

    public static string GetControllerName(ControllersNames controllerName)
    {
        return controllerName switch
        {
            ControllersNames.Employee => EMPLOYEE,
            ControllersNames.FirmClient => FIRM_CLIENT,
            ControllersNames.WorkOrder => WORK_ORDER,
            ControllersNames.Activity => ACTIVITY,
            ControllersNames.WorkTime => WORK_TIME,
            _ => string.Empty,
        };
    }
}

public enum ControllersNames
{
    Employee,
    FirmClient,
    WorkOrder,
    Activity,
    WorkTime
}