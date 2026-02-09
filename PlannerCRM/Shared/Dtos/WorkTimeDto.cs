using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;

public partial class WorkTimeDto
{
    public Guid? id { get; set; }
    public DateTime? creationDate { get; set; }

    [Required]
    [Description("Hours")]
    public int? hours { get; set; }
    public string? hoursString => hours != null ? hours.ToString() : "not set";
    public Guid? fkIdActivity { get; set; }
    public Guid? fkIdWorkOrder { get; set; }
    public Guid? fkIdFirmClient { get; set; }
    public Guid? fkIdEmployee { get; set; }
    public ActivityDto? fkIdActivityNavigation { get; set; }
    public WorkOrderDto? fkIdWorkOrderNavigation { get; set; }
}

public class WorkTimeFilterDto : FilterDto
{
    public Guid? activityId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? employeeId { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
}
