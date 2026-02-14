using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PlannerCRM.Shared.Dtos;

public class SystemLogDto
{
    public Guid? id { get; set; }

    [Required]
    [Description("Endpoint")]
    [MaxLength(500)]
    public string? endpoint { get; set; }

    [Required]
    [Description("Reason")]
    [MaxLength(5000)]
    public string? reason { get; set; }

    [Required]
    [Description("Stack Trace")]
    [MaxLength(5000)]
    public string? stacktrace { get; set; }

    [Required]
    [Description("Event Date")]
    public DateTime? date { get; set; }

    [Required]
    [Description("Username")]
    [MaxLength(50)]
    public string? username { get; set; }

    public string? request { get; set; }

    public Guid? fkIdProject { get; set; }

    public ProjectDto? fkIdProjectNavigation { get; set; }
}

public partial class SystemLogFilterDto : FilterDto
{
    public Guid? fkIdProject { get; set; }
    public DateTime? date { get; set; }
    public string? username { get; set; }

}