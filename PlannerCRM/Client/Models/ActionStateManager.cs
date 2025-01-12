namespace PlannerCRM.Client.Models;

public enum ActionState
{
    None,
    Add,
    Update,
    DeleteSingle,
    DeleteMultiple
}

public class ActionStateManager
{
    public ActionState CurrentState { get; set; } = ActionState.None;
    public bool IsOperationDone { get; set; } = false;
}
