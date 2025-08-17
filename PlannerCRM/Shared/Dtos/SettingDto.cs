using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class SettingDto
{
    public Guid id { get; set; }

    public string title { get; set; }

    public string description { get; set; }

    public bool isActive { get; set; }

    public bool isAdvanced { get; set; }
}

public class SettingFilterDto : FilterDto
{

}

public partial class ApiUrl
{
    public const string SETTINGS_CONTROLLER = "api/settings";

    public const string SETTINGS_INSERT = "insert";
    public const string SETTINGS_UPDATE = "update";
    public const string SETTINGS_DELETE = "delete";
    public const string SETTINGS_GET = "get";
    public const string SETTINGS_LIST = "list";
}