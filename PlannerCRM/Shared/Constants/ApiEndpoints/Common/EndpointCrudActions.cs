namespace PlannerCRM.Shared.Constants.ApiEndpoints.Common;

public class EndpointCrudActions
{
    public const string ADD = "add";
    public const string EDIT = "edit";
    public const string DELETE = "delete";
    public const string GET_BY_ID = "getById/{itemid1}";
    public const string GET_WITH_PAGINATION = "getWithPagination/{limit}/{offset}";

    public const string GET_BY_ID_ACTION = $"getById";
    public const string GET_WITH_PAGINATION_ACTION = "getWithPagination";
}