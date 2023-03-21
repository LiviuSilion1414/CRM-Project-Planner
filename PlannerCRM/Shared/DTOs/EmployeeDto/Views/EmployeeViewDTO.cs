namespace PlannerCRM.Shared.DTOs.EmployeeDto.Views;

public class EmployeeViewDTO
{ 
    public int Id { get; set; }

    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public string Birthday { get; set; }
    public decimal HourlyPay { get; set; }
}
