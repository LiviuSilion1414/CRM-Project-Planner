using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models2;

public partial class Activity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid WorkOrderId { get; set; }

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual WorkOrder WorkOrder { get; set; }

    public virtual ICollection<WorkOrderActivity> WorkOrderActivities { get; set; } = new List<WorkOrderActivity>();

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
