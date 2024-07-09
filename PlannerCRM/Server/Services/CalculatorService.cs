namespace PlannerCRM.Server.Services;

public class CalculatorService(AppDbContext dbContext)
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<int> GetCollectionSizeAsync() => await _dbContext.WorkOrderCosts.CountAsync();

    public async Task AddInvoiceAsync(int workOrderId) {
        var isAnyWorkOrder = await _dbContext.WorkOrders
            .AnyAsync(wo => wo.Id == workOrderId);

        var isAnyInvoice = await _dbContext.WorkOrderCosts
            .AnyAsync(inv => inv.WorkOrderId == workOrderId);

        if (!isAnyWorkOrder) {
            throw new KeyNotFoundException(ExceptionsMessages.WORKORDER_NOT_FOUND);
        }

        if (isAnyInvoice) {
            throw new DuplicateElementException(ExceptionsMessages.DUPLICATE_OBJECT);
        }

        var workOrder = await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);

        workOrder.IsCompleted = true;

        _dbContext.Update(workOrder);

        var workOrderCost = await ExecuteCalculationsAsync(workOrder, true);

        await _dbContext.WorkOrderCosts.AddAsync(workOrderCost);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersCostsAsync(int limit, int offset) {
        return await _dbContext.WorkOrders
            .OrderBy(wo => wo.Id)
            .Skip(limit)
            .Take(offset)
            .Select(wo => wo.MapToWorkOrderViewDto(_dbContext))
            .ToListAsync();
    }

    public async Task<WorkOrderCostDto> GetWorkOrderCostForViewByIdAsync(int workOrderId) {
        if (workOrderId == 0) {
            throw new KeyNotFoundException(ExceptionsMessages.WORKORDER_NOT_FOUND);
        }

        var workOrder = await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);
        var workOrderCost = await ExecuteCalculationsAsync(workOrder);

        var employees = workOrderCost.Employees
            .Select(em => em.MapToEmployeeViewDto())
            .ToList();

        var activities = workOrderCost.Activities
            .Select(ac => ac.MapToActivityViewDto(_dbContext))
            .ToList();

        var monthlyActivityCosts = workOrderCost.MonthlyActivityCosts
            .Select(ac => ac.MapToActivityCostDto(employees))
            .ToList();

        return workOrderCost.MapToWorkOrderCostDto(employees, activities, monthlyActivityCosts);
    }

    private async Task SetInvoiceCreatedFlagAsync(WorkOrder workOrder) {
        workOrder.IsInvoiceCreated = true;

        _dbContext.Update(workOrder);

        await _dbContext.SaveChangesAsync();
    }

    private async Task<WorkOrderCost> ExecuteCalculationsAsync(WorkOrder workOrder, bool onCreate = false) {
        if (onCreate) {
            await SetInvoiceCreatedFlagAsync(workOrder);
        }

        var employees = await GetAllRelatedEmployeesAsync(workOrder.Id);
        var activities = await GetAllRelatedActivitiesAsync(workOrder.Id);
        var monthlyActivityCosts = await CalculateMonthlyCostAsync(workOrder.Id);
        var totalEmployees = await GetRelatedEmployeesSizeAsync(workOrder.Id);
        var totalActivities = await GetRelatedActivitiesSizeAsync(workOrder.Id);
        var totalHours = await CalculateTotalHoursAsync(workOrder.Id);
        var totalCost = (await CalculateMonthlyCostAsync(workOrder.Id))
            .Sum(cost => cost.MonthlyCost);
        var monthlyCost = (await CalculateMonthlyCostAsync(workOrder.Id))
            .Sum(cost => cost.MonthlyCost);
        var activitiesSize = await GetRelatedActivitiesSizeAsync(workOrder.Id);
        var costPerMonth = activitiesSize == 0
            ? 0
            : monthlyCost / activitiesSize;

        return workOrder.MapToWorkOrderCost(
            employees,
            activities,
            monthlyActivityCosts,
            totalEmployees,
            totalActivities,
            totalHours,
            totalCost,
            costPerMonth
        );
    }

    private async Task<int> CalculateTotalHoursAsync(int workOrderId) {
        return await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .SumAsync(x => x.Hours);
    }

    private async Task<int> GetRelatedEmployeesSizeAsync(int workOrderId) {
        return await _dbContext.Users
            .Where(em => _dbContext.WorkTimeRecords
                .Any(wtr => wtr.EmployeeId == em.Id && wtr.WorkOrderId == workOrderId))
            .CountAsync();
    }

    private async Task<int> GetRelatedActivitiesSizeAsync(int workOrderId) {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .CountAsync();
    }

    private async Task<List<Employee>> GetAllRelatedEmployeesAsync(int workOrderId) {
        return await _dbContext.Users
            .Where(em => _dbContext.EmployeeActivities
                .Any(ea => em.Id == ea.EmployeeId))
            .ToListAsync();
    }

    private async Task<List<Activity>> GetAllRelatedActivitiesAsync(int workOrderId) {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();
    }

    private async Task<List<ActivityCost>> CalculateMonthlyCostAsync(int workOrderId) {
        var workTimeRecords = await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .ToListAsync();

        var activities = await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        var salaries = await _dbContext.Users
            .Where(em => em.Salaries
                .Any(sal => sal.EmployeeId == em.Id) &&
                    _dbContext.WorkTimeRecords
                        .Any(wtr => wtr.EmployeeId == em.Id))
            .Select(em => em.MapToEmployeeSalary())
            .ToListAsync();

        return workTimeRecords
            .Where(wtr => salaries
                .Any(sal => sal.EmployeeId == wtr.EmployeeId))
            .Select(wtr => wtr.MapToActivityCost(_dbContext, activities, salaries))
            .ToList();
    }
}