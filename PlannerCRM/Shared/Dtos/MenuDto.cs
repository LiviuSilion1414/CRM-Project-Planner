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

    //[RequiredIf(nameof(isDropdown), false, ErrorMessage = "Path is required when 'Is Dropdown ?' is false")]
    [MaxLength(50)]
    [Description("Path")]
    [Required]
    public string? path { get; set; }

    [Required]
    [Description("Ranking")]
    public int? ranking { get; set; }

    public Guid? idParent { get; set; }

    public virtual ICollection<MenuRoleDto> menuRoles { get; set; } = new List<MenuRoleDto>();

}

public class MenuNode
{
    public Guid? id { get; set; }
    public Guid? idParent { get; set; }

    public bool? isMenu { get; set; }
    public bool? isDropdown { get; set; }

    public string title { get; set; }
    public string icon { get; set; }
    public string path { get; set; }

    public List<MenuNode> Children { get; set; } = new();
}

public partial class MenuFilterDto : FilterDto
{
    public string? title { get; set; }
}