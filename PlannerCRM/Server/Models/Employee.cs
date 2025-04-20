using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string PasswordHash { get; set; }

    public string Phone { get; set; }

    public bool IsActive { get; set; }

    public bool LockoutEnd { get; set; }

    public DateOnly LastSeen { get; set; }

    public bool IsRemoveable { get; set; }

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
