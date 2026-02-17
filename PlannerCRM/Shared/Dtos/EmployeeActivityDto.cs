using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class EmployeeActivityDto
{
    public Guid? id { get; set; }
    public Guid? fkIdActivity { get; set; }
    public Guid? fkIdEmployee { get; set; }
    public Guid? fkIdWorkOrder { get; set; }
    public Guid? fkIdFirmClient { get; set; }
    public ActivityDto? fkIdActivityNavigation { get; set; }
    public EmployeeDto? fkIdEmployeeNavigation { get; set; }
    public FirmClientDto? fkIdFirmClientNavigation { get; set; }
    public WorkOrderDto? fkIdWorkOrderNavigation { get; set; }
}

public partial class EmployeeActivityFilterDto : FilterDto
{ 
    public Guid? activityId { get; set; }
    public Guid? employeeId { get; set; }
}