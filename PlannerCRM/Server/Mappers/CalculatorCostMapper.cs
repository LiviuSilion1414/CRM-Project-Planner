namespace PlannerCRM.Server.Mappers;

public static class CalculatorCostMapper
{
    public static ActivityCostDto MapToActivityCostDto(this ActivityCost activityCost, List<EmployeeViewDto> involvedEmployees)
    {
        return new ActivityCostDto
        {
            Id = activityCost.Id,
            Name = activityCost.Name,
            StartDate = activityCost.StartDate,
            FinishDate = activityCost.FinishDate,
            MonthlyCost = activityCost.MonthlyCost,
            Employees = involvedEmployees
        };
    }

    public static WorkOrderCostDto MapToWorkOrderCostDto(
        this WorkOrderCost workOrderCost,
        List<EmployeeViewDto> involvedEmployees,
        List<ActivityViewDto> activities,
        List<ActivityCostDto> monthlyActivityCosts)
    {
        return new WorkOrderCostDto
        {
            Id = workOrderCost.Id,
            WorkOrderId = workOrderCost.WorkOrderId,
            Name = workOrderCost.Name,
            StartDate = workOrderCost.StartDate,
            FinishDate = workOrderCost.FinishDate,
            TotalTime = workOrderCost.FinishDate - workOrderCost.StartDate,
            ClientId = workOrderCost.ClientId,
            IsInvoiceCreated = workOrderCost.IsCreated,
            Employees = involvedEmployees,
            Activities = activities,
            MonthlyActivityCosts = monthlyActivityCosts,
            TotalEmployees = workOrderCost.TotalEmployees,
            TotalActivities = workOrderCost.TotalActivities,
            TotalHours = workOrderCost.TotalHours,
            TotalCost = workOrderCost.TotalCost,
            CostPerMonth = workOrderCost.CostPerMonth
        };
    }

    public static WorkOrderCost MapToWorkOrderCost(
        this WorkOrder workOrder,
        List<Employee> employees,
        List<Activity> activities,
        List<ActivityCost> monthlyActivityCosts,
        int totalEmployees,
        int totalActivities,
        int totalHours,
        decimal totalCost,
        decimal costPerMonth
        )
    {
        return new WorkOrderCost
        {
            WorkOrderId = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            TotalTime = workOrder.FinishDate - workOrder.StartDate,
            IsCreated = true,
            IssuedDate = DateTime.Now,
            ClientId = workOrder.ClientId,
            Employees = employees,
            Activities = activities,
            MonthlyActivityCosts = monthlyActivityCosts,
            TotalEmployees = totalEmployees,
            TotalActivities = totalActivities,
            TotalHours = totalHours,
            TotalCost = totalCost,
            CostPerMonth = costPerMonth
        };
    }

    public static EmployeeSalary MapToEmployeeSalary(this Employee employee)
    {
        return new EmployeeSalary
        {
            EmployeeId = employee.Id,
            StartDate = employee.Salaries
                .Single(sal => sal.EmployeeId == employee.Id)
                .StartDate,
            FinishDate = employee.Salaries
                .Single(sal => sal.EmployeeId == employee.Id)
                .FinishDate,
            Salary = employee.Salaries
                .Single(sal => sal.EmployeeId == employee.Id)
                .Salary
        };
    }

    public static ActivityCost MapToActivityCost(
        this WorkTimeRecord workTimeRecord,
        AppDbContext context,
        List<Activity> activities,
        List<EmployeeSalary> salaries)
    {
        return new ActivityCost
        {
            Id = workTimeRecord.Id,
            Name = activities
                .First(ac => ac.WorkOrderId == workTimeRecord.WorkOrderId)
                .Name,
            StartDate = activities
                .First(ac => ac.WorkOrderId == workTimeRecord.WorkOrderId)
                .StartDate,
            FinishDate = activities
                .First(ac => ac.WorkOrderId == workTimeRecord.WorkOrderId)
                .FinishDate,
            Employees = context.Users
                .Where(em => em.Id == workTimeRecord.EmployeeId)
                .ToList(),
            MonthlyCost = salaries
                .First(ems => ems.EmployeeId == workTimeRecord.EmployeeId)
                .Salary * workTimeRecord.Hours
        };
    }
}