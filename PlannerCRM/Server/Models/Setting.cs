using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Setting
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsAdvanced { get; set; }
}
