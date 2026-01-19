using System;
using System.Collections.Generic;

namespace PlannerCRM.Server.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Username { get; set; }

    public DateTime? CreationDate { get; set; }

    public string PasswordHash { get; set; }

    public DateTime? LastSeen { get; set; }

    public bool? IsRemoveable { get; set; }

    public string Token { get; set; }

    public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = new List<EmployeeActivity>();

    public virtual ICollection<EmployeesRole> EmployeesRoles { get; set; } = new List<EmployeesRole>();

    public virtual ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
}
