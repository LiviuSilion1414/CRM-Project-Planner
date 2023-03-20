using PlannerCRM.Shared.DTOs.Abstract;

namespace PlannerCRM.Shared.DTOs;

public class EmployeeViewDTO : EmployeeDTO
{ 
    public string NumericCode { get; set; }
    public string FullName { get; set; }
    public string Birthday { get; set; }
    public decimal HourlyPay { get; set; }
}
