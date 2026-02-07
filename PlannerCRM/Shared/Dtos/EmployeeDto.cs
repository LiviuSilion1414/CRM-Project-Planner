using PlannerCRM.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Dtos;

public partial class EmployeeDto
{
    public Guid? id { get; set; }

    [Description("Name")]
    [Required]
    public string? name { get; set; }

    [Description("Surname")]
    [Required]
    public string? surname { get; set; }

    [Description("Username")]
    [Required]
    [EmailAddress]
    public string? username { get; set; }

    [Description("Can remove?")]
    [Required(ErrorMessage = "Check this box to continue")]
    public bool? isRemoveable { get; set; }

    public string? fullname => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname) ? surname + " " + name : string.Empty;

    public DateTime? creationDate { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "";

    //[Required]
    public string? newPassword { get; set; }

    //[Required]
    //[Compare(nameof(newPassword), ErrorMessage = "The new password and confirmation password do not match.")]
    public string? confirmNewPassword { get; set; }

    public bool isComplete => !string.IsNullOrEmpty(name) 
                              && !string.IsNullOrEmpty(surname) 
                              && !string.IsNullOrEmpty(username) 
                              && isRemoveable != null 
                              && employeesRoles != null 
                              && employeesRoles.Any();

    public DateTime? lastSeen { get; set; }
    public string? lastSeenString => lastSeen != null ? string.Format("{0:dd/MM/yyyy}", lastSeen) : "";


    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public ICollection<EmployeesRolesDto>? employeesRoles { get; set; }
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
    public string? username { get; set; }
    public string? name { get; set; }
    public string? surname { get; set; }

}