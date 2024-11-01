
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Services;

public class CalculatorService(AppDbContext dbContext)
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<int> GetCollectionSizeAsync() => await _dbContext.WorkOrderCosts.CountAsync();

    public async Task AddInvoiceAsync(int workOrderId)
    {
        var isAnyWorkOrder = await _dbContext.WorkOrders
            .AnyAsync(wo => wo.Id == workOrderId);

        var isAnyInvoice = await _dbContext.WorkOrderCosts
            .AnyAsync(inv => inv.WorkOrderId == workOrderId);

        if (!isAnyWorkOrder)
        {
            throw new KeyNotFoundException(ExceptionsMessages.WORKORDER_NOT_FOUND);
        }

        if (isAnyInvoice)
        {
            throw new DuplicateElementException(ExceptionsMessages.DUPLICATE_OBJECT);
        }

        var workOrder = await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);

        workOrder.IsCompleted = true;
        workOrder.IsInvoiceCreated = true;
        _dbContext.Update(workOrder);

        var workOrderCost = await ExecuteCalculationsAsync(workOrder, true);

        await _dbContext.WorkOrderCosts.AddAsync(workOrderCost);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<WorkOrderViewDto>> GetCompletedWorkOrdersAsync(int limit, int offset)
    {
        var workOrders = await _dbContext.WorkOrders
            .Where(wo => wo.IsCompleted)
            .OrderBy(wo => wo.Id)
            .Skip(limit)
            .Take(offset)
            .ToListAsync();

        foreach (var wo in workOrders)
        {
            wo.Client = await GetClientByIdAsync(wo.ClientId);
        }

        return workOrders
            .Select(wo => wo.MapToWorkOrderViewDto())
            .ToList();
    }

    public async Task<List<WorkOrderCostDto>> GetPaginatedWorkOrdersCostsAsync(int limit, int offset)
    {
        var workOrdersCosts = await _dbContext.WorkOrderCosts
            .OrderBy(wo => wo.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        
        var mappedWorkOrdersCosts = workOrdersCosts
            .Select(wo => wo.MapToWorkOrderCostDto())
            .ToList();
        
        foreach (var wo in mappedWorkOrdersCosts)
        {
            wo.Client = (await GetClientByIdAsync(wo.ClientId)).MapToClientViewDto();
            wo.WorkOrder = (await GetWorkOrderByIdAsync(wo.WorkOrderId)).MapToWorkOrderViewDto();
        }
                
        return mappedWorkOrdersCosts;

    }

    private async Task<WorkOrder> GetWorkOrderByIdAsync(int workOrderId)
    {
        return await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);
    }

    private async Task<FirmClient> GetClientByIdAsync(int workOrderId)
    {
        var workOrder = await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);

        return await _dbContext.Clients
            .SingleAsync(cl => cl.Id == workOrder.ClientId);
    }

    public async Task<WorkOrderCostDto> GetWorkOrderCostForViewByIdAsync(int workOrderId)
    {
        var workOrderCost = await _dbContext.WorkOrderCosts
            .SingleAsync(wo => wo.WorkOrderId == workOrderId);

        var employees = await _dbContext.Users
            .Where(em => _dbContext.WorkTimeRecords
                .Any(wtr => wtr.EmployeeId == em.Id && wtr.WorkOrderId == workOrderId))
            .ToListAsync();
        var mappedEmployees = employees
            .Select(em => em.MapToEmployeeViewDto())
            .ToList();

        var activities = await _dbContext.Activities
            .Where(ac => _dbContext.WorkTimeRecords
                .Any(wtr => wtr.ActivityId == ac.Id && wtr.WorkOrderId == workOrderId))
            .ToListAsync();

        var activitiesWithEmployeeActivities = new List<Activity>();
        foreach (var ac in activities)
        {
            activitiesWithEmployeeActivities.Add(await GetEmployeesInvolvedInSingleActivity(ac.Id));
        }
        
        var mappedActivities = activitiesWithEmployeeActivities
            .Select(ac => ac.MapToActivityViewDto())
            .ToList();

        var monthlyActivityCosts = await CalculateMonthlyCostAsync(workOrderId);
        var mappedMonthlyActivityCosts = monthlyActivityCosts
            .Select(mac => mac.MapToActivityCostDto(mappedEmployees))
            .ToList();

        return workOrderCost.MapToWorkOrderCostDto(mappedEmployees, mappedActivities, mappedMonthlyActivityCosts);
    }

    private async Task<Activity> GetEmployeesInvolvedInSingleActivity(int activityId)
    {
        var activity = await _dbContext.Activities
            .SingleAsync(ac => ac.Id == activityId);
        activity.EmployeeActivity = (await GetEmployeesActivitiesByActivityId(activity.Id)).ToHashSet();
        foreach (var ea in activity.EmployeeActivity)
        {
            ea.Employee = await _dbContext.Users
                .SingleAsync(e => e.Id == ea.EmployeeId);
        }
        return activity;
    }

    private async Task<List<EmployeeActivity>> GetEmployeesActivitiesByActivityId(int activityId)
    {
        return await _dbContext.EmployeeActivities
            .Where(ea => ea.ActivityId == activityId)
            .ToListAsync();
    }

    private async Task<WorkOrderCost> ExecuteCalculationsAsync(WorkOrder workOrder, bool onCreate = false)
    {
        if (onCreate)
        {
            workOrder.IsInvoiceCreated = true;

            _dbContext.Update(workOrder);
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

    private async Task<int> CalculateTotalHoursAsync(int workOrderId)
    {
        return await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .SumAsync(x => x.Hours);
    }

    private async Task<int> GetRelatedEmployeesSizeAsync(int workOrderId)
    {
        return await _dbContext.Users
            .Where(em => _dbContext.WorkTimeRecords
                .Any(wtr => wtr.EmployeeId == em.Id && wtr.WorkOrderId == workOrderId))
            .CountAsync();
    }

    private async Task<int> GetRelatedActivitiesSizeAsync(int workOrderId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .CountAsync();
    }

    private async Task<List<Employee>> GetAllRelatedEmployeesAsync(int workOrderId)
    {
        return await _dbContext.Users
            .Where(em => _dbContext.EmployeeActivities
                .Any(ea => em.Id == ea.EmployeeId))
            .ToListAsync();
    }

    private async Task<List<Activity>> GetAllRelatedActivitiesAsync(int workOrderId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();
    }

    private async Task<List<ActivityCost>> CalculateMonthlyCostAsync(int workOrderId)
    {
        var workTimeRecords = await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .ToListAsync();

        var activities = await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        var employeesSalariesInWorkOrder = await _dbContext.EmployeeSalaries
            .Where(sal => _dbContext.Users
                .Any(em => em.Id == sal.EmployeeId))
            .ToListAsync();

        return workTimeRecords
            .Where(wtr => employeesSalariesInWorkOrder
                .Any(sal => sal.EmployeeId == wtr.EmployeeId))
            .Select(wtr => wtr.MapToActivityCost(_dbContext, activities, employeesSalariesInWorkOrder))
            .ToList();
    }

    public async Task DeleteInvoiceAsync(int workOrderId)
    {
        var workOrderCost = await _dbContext.WorkOrderCosts
            .SingleAsync(w => w.WorkOrderId == workOrderId);

        var workOrder = await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);

        _dbContext.WorkOrderCosts.Remove(workOrderCost);

        workOrder.IsInvoiceCreated = false;

        _dbContext.Update(workOrder);

        await _dbContext.SaveChangesAsync();
    }
}