using System;
using System.Collections.Generic;
using System.Text;

namespace PlannerCRM.Shared.Dtos;

public class SystemLogDto
{
    public Guid? id { get; set; }

    public string? endpoint { get; set; }

    public string? reason { get; set; }

    public string? stacktrace { get; set; }

    public DateTime? date { get; set; }

    public string? username { get; set; }

    public string? request { get; set; }

    public Guid? fkIdProject { get; set; }

    public virtual ProjectDto? fkIdProjectNavigation { get; set; }
}

public partial class SystemLogFilterDto : FilterDto
{
    public Guid? fkIdProject { get; set; }
    public DateTime? dateFrom { get; set; }
    public DateTime? dateTo { get; set; }
    public string? username { get; set; }

}