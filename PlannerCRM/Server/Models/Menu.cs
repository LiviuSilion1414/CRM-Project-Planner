using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Menu
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Icon { get; set; }

    public string Path { get; set; }

    public bool? IsDropdown { get; set; }

    public bool? IsMenu { get; set; }

    public int? Ranking { get; set; }

    public Guid? IdParent { get; set; }

    public virtual Menu IdParentNavigation { get; set; }

    public virtual ICollection<Menu> InverseIdParentNavigation { get; set; } = new List<Menu>();

    public virtual ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
}
