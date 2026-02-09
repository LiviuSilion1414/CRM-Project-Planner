using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;

public partial class WorkOrderDto
{
    public Guid? id { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Name")]
    public string? name { get; set; }

    [Required]
    [MaxLength(2000)]
    [Description("Description")]
    public string? description { get; set; }
    public DateTime? creationDate { get; set; }
    public string? creationDateString => creationDate != null ? string.Format("{0:dd/MM/yyyy}", creationDate) : "not set";
    public DateTime? startDate { get; set; }
    public string? startDateString => startDate != null ? string.Format("{0:dd/MM/yyyy}", startDate) : "not set";
    public DateTime? endDate { get; set; }
    public string? endDateString => endDate != null ? string.Format("{0:dd/MM/yyyy}", endDate) : "not set";
    public Guid? fkIdFirmClient { get; set; }
    public ICollection<ActivityDto>? activities { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public ICollection<WorkTimeDto>? workTimes { get; set; }
}

public class WorkOrderFilterDto : FilterDto
{
    public Guid? workOrderId { get; set; }
    public Guid? firmClientId { get; set; }
    public Guid? employeeId { get; set; }
}
