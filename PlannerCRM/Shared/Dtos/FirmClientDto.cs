using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;

public partial class FirmClientDto
{
    public Guid? id { get; set; }

    [Required]
    [Description("Name")]
    [MaxLength(50)]
    public string? name { get; set; }

    [Required]
    [Description("Vat Number")]
    [MaxLength(50)]
    public string? vatNumber { get; set; }

    [Required]
    [EmailAddress]
    [Description("Email")]
    [MaxLength(50)]
    public string? email { get; set; }

    [Required]
    [Description("Fiscal code")]
    [MaxLength(20)]
    public string? fiscalCode { get; set; }
    
    public ICollection<ActivityDto>? activities { get; set; }
    public ICollection<EmployeeActivityDto>? employeeActivities { get; set; }
    public ICollection<WorkOrderDto>? workOrders { get; set; }
}

public class FirmClientFilterDto : FilterDto
{
    public Guid? firmClientId { get; set; }
    public Guid? workOrderId { get; set; }
    public Guid? workOrderCostId { get; set; }
    public string? name { get; set; }
    public string? fiscalCode { get; set; }
    public string? email { get; set; }
    public string? vatNumber { get; set; }

}
