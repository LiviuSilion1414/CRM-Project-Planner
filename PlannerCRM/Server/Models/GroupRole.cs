using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class GroupRole
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
