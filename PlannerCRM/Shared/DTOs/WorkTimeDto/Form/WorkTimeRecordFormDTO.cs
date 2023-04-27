using PlannerCRM.Shared.DTOs.EmployeeDto.Views;

namespace PlannerCRM.Shared.DTOs.WorkTimeDto.Form;

public class WorkTimeRecordFormDTO
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
    public Decimal TotalPrice { get; set; }

    public int ActivityId { get; set; }
    public int WorkOrderId { get; set; }

    public int EmployeeId { get; set; }
}