using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class MenuDto
{
    public Guid id { get; set; }

    public string title { get; set; }

    public bool isDropdown { get; set; }

    public string icon { get; set; }

    public string path { get; set; }
}
