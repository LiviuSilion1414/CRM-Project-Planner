using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlannerCRM.Shared.Dtos;
public static class DtoShared
{
    public static string GetDescription(this Enum value)     
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
        return attributes?.Description;
    }

    public enum ActionType
    {
        INSERT,
        UPDATE,
        DELETE,
        DELETE_MULTIPLE
    }

    public enum ColumnActionType
    {
        ADVANCED_UPDATE_READONLY,
        ADVANCED_UPDATE,
        UPDATE,
        DELETE,
        VIEW
    }

    public enum MenuViewType
    {
        TREE_VIEW,
        PANEL_MENU
    }

    public enum Projects
    {
        [Description("A114651F-3F6F-408D-8477-60087AB27A6D")] Api,
        [Description("80051D73-9672-47EA-A616-65D03F7A67D1")] Backoffice,
    }

    public enum BaseRoles
    {
        [Description("66FBECFE-5CEF-4ACE-9A04-23AFBAF1A3C4")] User,
        [Description("2E926644-0F08-4204-B92F-38AC9CA18E66")] Tech_Assistance,
        [Description("E4FEDDE3-2A93-4A65-98E5-8F927BDB1C61")] Operation_Manager,
        [Description("BA157D28-F713-462C-9FA6-C9FF17E016AB")] Admin,
        [Description("0C83C028-705A-491E-9D0E-D1B75518A04E")] ProjectManager,
    }

    public enum BaseRolesStrings
    {
        [Description("User")] User,
        [Description("Tech Assistance")] Tech_Assistance,
        [Description("Operation Manager")] Operation_Manager,
        [Description("Admin")] Admin,
        [Description("Project Manager")] ProjectManager,
    }

    public static List<string> BaseRolesList = Enum.GetValues<BaseRolesStrings>().Select(x => x.GetDescription()).ToList();

    public partial class ApiUrl
    {
        public const string ACCOUNT_CONTROLLER = "api/account/";
        public const string ACTIVITY_CONTROLLER = "api/activity/";
        public const string CLIENT_CONTROLLER = "api/client/";
        public const string EMPLOYEE_CONTROLLER = "api/employee/";
        public const string EMPLOYEES_ROLES_CONTROLLER = "api/employeesroles/";
        public const string EMPLOYEES_ACTIVITIES_CONTROLLER = "api/employeesactivities/";
        public const string MENU_ROLE_CONTROLLER = "api/menurole/";
        public const string GROUP_ROLES_CONTROLLER = "api/grouproles/";
        public const string ROLE_CONTROLLER = "api/role/";
        public const string MENU_CONTROLLER = "api/menu/";
        public const string SETTINGS_CONTROLLER = "api/settings/";
        public const string SYSTEMLOG_CONTROLLER = "api/systemlog/";
        public const string WORKORDER_CONTROLLER = "api/workorder/";
        public const string WORKTIME_CONTROLLER = "api/worktime/";
        public const string MAIL_SENDER_CONTROLLER = "api/mailsender/";

        public const string ACCOUNT_LOGIN = "login";

        public const string INSERT = "insert";
        public const string UPDATE = "update";
        public const string DELETE = "delete";
        public const string DELETE_MULTIPLE = "deleteMultiple";
        public const string GET = "get";
        public const string LIST = "list";

        public const string MENU_LIST_BY_EMPLOYEE_ID = "menuListByEmployeeId";
        public const string ROLE_LIST_BY_EMPLOYEE_ID = "roleListByEmployeeId";

        public const string SEND_MAIL = "sendMail";
    }
}
