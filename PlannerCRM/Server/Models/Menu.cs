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

    public int? Ranking { get; set; }

    public virtual ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
}
