using PlannerCRM.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Dtos;

public partial class EmployeeDto
{
    public Guid? id { get; set; }

    [Required]
    public string? name { get; set; }

    [Required]
    public string? surname { get; set; }

    [Required]
    [EmailAddress]
    public string? username { get; set; }
    public string? fullname => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname) ? surname + " " + name : string.Empty;
    public DateTime? creationDate { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "";

    [Required]
    public string? newPassword { get; set; }

    [Required]
    [Compare("newPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string? confirmNewPassword { get; set; }

    public DateTime? lastSeen { get; set; }
    public string? lastSeenString => lastSeen != null ? string.Format("{0:dd/MM/yyyy}", lastSeen) : "";

    [Required(ErrorMessage = "Is removeable? option is required")]
    public bool? isRemoveable { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public ICollection<EmployeesRolesDto>? employeesRoles { get; set; }
    public string employeesRolesCommaSeparatedString => employeesRoles != null && employeesRoles.Any()
        ? string.Join(", ", employeesRoles.Select(er => er?.fkIdRoleNavigation?.name))
        : "no rolesList set";
}

public class EmployeeFilterDto : FilterDto
{
    public Guid? employeeId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? activityId { get; set; }
    public Guid? roleId { get; set; }
    public bool? isRemoveRole { get; set; }
    public bool? isEditProfile { get; set; }
    public RoleDto? role { get; set; }
}