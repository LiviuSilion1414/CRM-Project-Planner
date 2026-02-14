using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();
}
