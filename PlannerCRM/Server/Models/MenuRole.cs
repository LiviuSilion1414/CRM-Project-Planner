using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class MenuRole
{
    public Guid Id { get; set; }

    public Guid FkIdMenu { get; set; }

    public Guid FkIdRole { get; set; }

    public virtual Menu FkIdMenuNavigation { get; set; }

    public virtual Role FkIdRoleNavigation { get; set; }
}
