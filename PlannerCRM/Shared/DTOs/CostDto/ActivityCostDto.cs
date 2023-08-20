namespace PlannerCRM.Shared.DTOs.CostDto;

public class ActivityCostDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string StartDate { get; set; }
    public decimal FinishDate { get; set; }
    public List<EmployeeSelectDto> Employees { get; set; }
    public decimal MonthlyCost { get; set; }
}