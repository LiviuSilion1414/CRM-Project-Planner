using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;

public partial class SettingDto
{
    public Guid id { get; set; }

    [Required]
    [MaxLength(100)]
    [Description("Title")]
    public string? title { get; set; }

    [Required]
    [MaxLength(300)]
    [Description("Description")]
    public string? description { get; set; }

    [Required]
    [Description("Is active?")]
    public bool isActive { get; set; }

    [Required]
    [Description("Is advanced?")]
    public bool? isAdvanced { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Key")]
    public string? key { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Value")]
    public string? value { get; set; }
}

public class SettingFilterDto : FilterDto
{

}
