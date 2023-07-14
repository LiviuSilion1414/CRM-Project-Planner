namespace PlannerCRM.Shared.DTOs.ActivityDto.Views;

public class ActivitySelectHelperDto
{ 
    public string SelectedWorkorder { get; set; }
    public string SelectedEmployee { get; set; }
    public WorkOrderSelectDto WorkorderDto { get; set; }
    public EmployeeSelectDto EmployeeDto { get; set; }
}