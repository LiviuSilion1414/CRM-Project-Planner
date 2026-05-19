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

public static class MenuTreeBuilder
{
    public static List<MenuNode> Build(List<MenuDto>? menuList)
    {
        List<MenuNode> menuTree = new();
        var list = menuList ?? new();

        var lookup = list.ToDictionary(
            x => x.id,
            x => new MenuNode
            {
                id = x.id,
                idParent = x.idParent,
                title = x.title,
                icon = x.icon,
                path = x.path,
                isDropdown = x.isDropdown,
                isMenu = x.isMenu

            });

        foreach (var node in lookup.Values)
        {
            if (node.idParent != null && lookup.ContainsKey(node.idParent.Value))
            {
                lookup[node.idParent.Value].Children.Add(node);
            } else
            {
                menuTree.Add(node);
            }
        }

        return menuTree;
    }
}

public partial class MenuFilterDto : FilterDto
{
    public string? title { get; set; }

    public Guid? idParent { get; set; }

}