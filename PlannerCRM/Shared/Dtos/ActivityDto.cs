using PlannerCRM.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace PlannerCRM.Shared.Dtos;

public partial class ActivityDto
{
    public Guid? id { get; set; }

    [Required]
    [Description("Name")]
    [MaxLength(100)]
    public string? name { get; set; }

    public DateTime? creationDate { get; set; }
    //public string? description { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "not set";

    [Required]
    [Description("Start Date")]
    public DateTime? startDate { get; set; }
    public string? startDateString => startDate != null ? string.Format("{0:dd/MM/yyyy}", startDate) : "not set";

    [Required]
    [Description("End Date")]
    public DateTime? endDate { get; set; }
    public string? endDateString => endDate != null ? string.Format("{0:dd/MM/yyyy}", endDate) : "not set";
    public int? totalEmployeesInt => employeeActivities != null && employeeActivities.Any() ? employeeActivities.Count : 0;
    public Guid? fkIdWorkOrder { get; set; }

    //[MaxLength(50)]
    public string? hexColor { get; set; } = "#0000FF"; // Default color blue
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public WorkOrderDto? fkIdWorkOrderNavigation { get; set; }
    public ICollection<WorkTimeDto>? workTimes { get; set; }
}

public class ActivityFilterDto : FilterDto
{
    public Guid? activityId { get; set; }
    public Guid? employeeId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? firmClientId { get; set; }
}
