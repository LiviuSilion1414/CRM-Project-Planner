namespace PlannerCRM.Shared.Constants.ApiEndpoints.Common;

public class EndpointCrudActions
{
    public const string ADD = "add";
    public const string EDIT = "edit";
    public const string DELETE = "delete";
    public const string GET_BY_ID = $"getById/{EndpointsPlaceholders.ITEM_ID_1}";
    public const string GET_WITH_PAGINATION = $"getWithPagination/{EndpointsPlaceholders.LIMIT_OFFSET}";
}