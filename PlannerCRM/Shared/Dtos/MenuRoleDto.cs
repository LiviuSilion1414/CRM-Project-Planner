using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class MenuRoleDto
{
    public Guid id { get; set; }

    public Guid fkIdMenu { get; set; }

    public Guid fkIdRole { get; set; }

    public virtual MenuDto fkIdMenuNavigation { get; set; }

    public virtual RoleDto fkIdRoleNavigation { get; set; }
}

public partial class MenuRoleFilterDto : FilterDto
{
    public Guid? id { get; set; }
    public Guid? menuId { get; set; }
    public Guid? roleId { get; set; }
}

public partial class ApiUrl
{
    public const string MENU_ROLE_CONTROLLER = "api/menurole";

    public const string MENU_ROLE_INSERT = "insert";
    public const string MENU_ROLE_UPDATE = "update";
    public const string MENU_ROLE_DELETE = "delete";
    public const string MENU_ROLE_GET = "get";
    public const string MENU_ROLE_LIST = "list";
}