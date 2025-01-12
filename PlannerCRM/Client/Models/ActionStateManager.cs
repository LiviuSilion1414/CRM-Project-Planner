namespace PlannerCRM.Client.Models;

public enum ActionType
{
    None,
    View,
    Add,
    Update,
    DeleteSingle,
    DeleteMultiple
}

public class ActionStateManager
{
    public bool IsAddSelected { get; private set; }
    public bool IsViewSelected { get; private set; }
    public bool IsUpdateSelected { get; private set; }
    public bool IsDeleteSingleSelected { get; private set; }
    public bool IsDeleteMultipleSelected { get; private set; }
    public bool IsOperationDone { get; set; }

    public void SetAction(ActionType actionType)
    {
        ResetFlags();

        switch (actionType)
        {
            case ActionType.View:
                IsViewSelected = true; 
                break;
            case ActionType.Add:
                IsAddSelected = true;
                break;
            case ActionType.Update:
                IsUpdateSelected = true;
                break;
            case ActionType.DeleteSingle:
                IsDeleteSingleSelected = true;
                break;
            case ActionType.DeleteMultiple:
                IsDeleteMultipleSelected = true;
                break;
            case ActionType.None:
            default:
                break;
        }
    }

    private void ResetFlags()
    {
        IsAddSelected = false;
        IsUpdateSelected = false;
        IsDeleteSingleSelected = false;
        IsDeleteMultipleSelected = false;
    }
}
