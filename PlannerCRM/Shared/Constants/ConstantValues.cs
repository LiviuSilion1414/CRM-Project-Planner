using PlannerCRM.Shared.Models;

namespace PlannerCRM.Shared.Constants;

public static class ConstantValues
{
    public static DateTime CURRENT_DATE = DateTime.Now;
    public const int MIN_WORKORDER_MONTH_CONTRACT = 3;
    public const int MAX_WORKORDER_MONTH_CONTRACT = 24;
    public const int MIN_ACTIVITY_MONTH_PERIOD = 1;
    public const int MAX_ACTIVITY_MONTH_PERIOD = 6;
    public const int INVALID_ID = -1;
    public const string ADMIN_EMAIL = "account.manager@gmail.com";
    public const Roles ADMIN_ROLE = Roles.ACCOUNT_MANAGER;
    public const int MAJOR_AGE = 18;
    public const int MAX_AGE = 50;
    public const int MINIMUM_YEAR = 1973;
    public const int CURRENT_YEAR = 2023;
    public const int PASS_MIN_LENGTH = 8;
    public const int PASS_MAX_LENGTH = 16;
    public const int PAGINATION_LIMIT = 5;
    public const int ZERO = 0;
    public const int ONE = 1;
}