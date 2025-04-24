using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Username { get; set; }

    public DateOnly CreationDate { get; set; }

    public string PasswordHash { get; set; }

    public DateOnly LastSeen { get; set; }

    public virtual ICollection<ActivitiesWorkTime> ActivitiesWorkTimes { get; set; } = new List<ActivitiesWorkTime>();

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeeWorkTime> EmployeeWorkTimes { get; set; } = new List<EmployeeWorkTime>();

    public virtual ICollection<EmployeesRole> EmployeesRoles { get; set; } = new List<EmployeesRole>();
}
