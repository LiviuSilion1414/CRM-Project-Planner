using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class SystemLog
{
    public Guid Id { get; set; }

    public string Endpoint { get; set; }

    public string Reason { get; set; }

    public string Stacktrace { get; set; }

    public DateTime? Date { get; set; }

    public string Username { get; set; }

    public string Request { get; set; }

    public Guid? FkIdProject { get; set; }

    public virtual Project FkIdProjectNavigation { get; set; }
}
