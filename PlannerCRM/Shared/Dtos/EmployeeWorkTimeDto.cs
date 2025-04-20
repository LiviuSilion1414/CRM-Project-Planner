using System;
using System.Collections.Generic;

namespace PlannerCRM.Shared.Dtos;

public partial class EmployeeWorkTimeDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid WorkTimeId { get; set; }

    public Guid WorkOrderId { get; set; }

    public Guid ActivityId { get; set; }

    public ActivityDto Activity { get; set; }

    public EmployeeDto Employee { get; set; }

    public WorkOrderDto WorkOrder { get; set; }

    public WorkTimeDto DtoWorkTime { get; set; }
}
