using PlannerCRM.Shared.Attributes;
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
    [Description("Is Menu?")]
    public bool? isMenu { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Icon")]
    public string? icon { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Path")]
    [StartsWithSlash]
    public string? path { get; set; }

    [Required]
    [Description("Ranking")]
    public int? ranking { get; set; }

    public Guid? idParent { get; set; }

    public virtual ICollection<MenuRoleDto> menuRoles { get; set; } = new List<MenuRoleDto>();

}

public partial class MenuFilterDto : FilterDto
{
    public string? title { get; set; }
}