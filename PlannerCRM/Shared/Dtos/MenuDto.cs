using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;

public partial class MenuDto
{
    public Guid id { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Title")]
    public string? title { get; set; }

    [Required]
    [Description("Is Dropdown?")]
    public bool? isDropdown { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Icon")]
    public string? icon { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Path")]
    public string? path { get; set; }
}
