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
