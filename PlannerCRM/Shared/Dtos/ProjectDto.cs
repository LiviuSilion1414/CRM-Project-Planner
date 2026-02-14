using System;
using System.Collections.Generic;
using System.Text;

namespace PlannerCRM.Shared.Dtos;

public partial class ProjectDto
{
    public Guid? id { get; set; }
    public string? title { get; set; }
}