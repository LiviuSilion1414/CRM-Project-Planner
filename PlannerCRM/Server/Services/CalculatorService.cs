namespace PlannerCRM.Server.Services;

public class CalculatorService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(AppDbContext dbContext, ILogger<CalculatorService> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> GetCollectionSizeAsync()
        => await _dbContext.WorkTimeRecords
            .CountAsync();

    public async Task AddInvoiceAsync(int workOrderId) {
        try {
            var isAnyWorkOrder = await _dbContext.WorkOrders
                .AnyAsync(wo => wo.Id == workOrderId);
    
            var isAnyInvoice = await _dbContext.WorkOrderCosts
                .AnyAsync(inv => inv.WorkOrderId == workOrderId);
    
            if (isAnyInvoice && !isAnyWorkOrder) {
                throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
            }

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == workOrderId);  
            var workOrderCost = await ExecuteCalculationsAsync(workOrder);

            await _dbContext.WorkOrderCosts.AddAsync(workOrderCost);
    
            if (await _dbContext.SaveChangesAsync() == 0) {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
    
            throw;
        } 
    }

    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersCostsAsync(int limit, int offset) {
        return await _dbContext.WorkOrders
            .OrderBy(wo => wo.Id)
            .Skip(limit)
            .Take(offset)
            .Select(wo => 
                new WorkOrderViewDto {
                    Id = wo.Id,
                    Name = wo.Name,
                    StartDate = wo.StartDate,
                    FinishDate = wo.FinishDate,
                    IsCompleted = wo.IsDeleted,
                    IsDeleted = wo.IsDeleted,
                    IsInvoiceCreated = _dbContext.WorkOrderCosts
                        .Any(workCost => workCost.WorkOrderId == wo.Id),
                    ClientId = wo.ClientId
                }
            )
            .ToListAsync();
    }

    public async Task<WorkOrderCostDto> GetWorkOrderCostForView(int workOrderId) {
        return await _dbContext.WorkOrderCosts
            .OrderBy(wo => wo.Id)
            .Where(wo => wo.Id == workOrderId)
            .Select(inv => 
                new WorkOrderCostDto {
                    Id = inv.Id,
                    WorkOrderId = inv.Id,
                    Name = inv.Name,
                    StartDate = inv.StartDate,
                    FinishDate = inv.FinishDate,
                    TotalTime = inv.FinishDate - inv.StartDate,
                    ClientId = inv.ClientId,
                    Employees = inv.Employees
                        .Select(em => 
                            new EmployeeViewDto {
                                Id = em.Id,
                                Email = em.Email,
                                FullName = em.FullName,
                                EmployeeSalaries = em.Salaries
                                    .Select(s => 
                                        new EmployeeSalaryDto {
                                            EmployeeId = s.EmployeeId,
                                            Salary = s.Salary,
                                            StartDate = s.StartDate,
                                            FinishDate = s.FinishDate
                                        }
                                    )
                                    .ToList()
                            }
                        )
                        .ToList(),
                    Activities = inv.Activities
                        .Select(ac => 
                            new ActivityViewDto {
                                Id = ac.Id,
                                Name = ac.Name,
                                StartDate = ac.StartDate,
                                FinishDate = ac.FinishDate,
                                WorkOrderId = ac.WorkOrderId
                            }
                        )
                        .ToList(),
                    MonthlyActivityCosts = inv.MonthlyActivityCosts
                        .Select(monthlyCost => 
                            new ActivityCostDto {
                                Name = monthlyCost.Name,
                                StartDate = monthlyCost.StartDate,
                                FinishDate = monthlyCost.FinishDate,
                                Employees = monthlyCost.Employees
                                    .Select(em =>
                                        new EmployeeViewDto {
                                            Id = em.Id,
                                            Email = em.Email,
                                            FullName = em.FullName
                                        }
                                    )
                                    .ToList()
                            }
                        )
                        .ToList(),
                    TotalEmployees = inv.TotalEmployees,
                    TotalActivities = inv.TotalActivities,
                    TotalHours = inv.TotalHours,
                    TotalCost = inv.TotalCost,
                    CostPerMonth = inv.CostPerMonth
                }
            )
            .SingleOrDefaultAsync();
    }

    private async Task SetInvoiceCreatedFlagAsync(WorkOrder workOrder) {
        workOrder.IsInvoiceCreated = true;

        _dbContext.Update(workOrder);

        if (await _dbContext.SaveChangesAsync() == 0) {
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
        }
    }

    private async Task<WorkOrderCost> ExecuteCalculationsAsync(WorkOrder workOrder) {
        try {
            await SetInvoiceCreatedFlagAsync(workOrder);
            
            var employees = await GetAllRelatedEmployeesAsync(workOrder.Id);
            var activities = await GetAllRelatedActivitiesAsync(workOrder.Id);
            var monthlyActivityCosts = await CalculateMonthlyCostAsync(workOrder.Id);
            var totalEmployees = await GetRelatedEmployeesSizeAsync(workOrder.Id);
            var totalActivities = await GetRelatedActivitiesSizeAsync(workOrder.Id);
            var totalHours = await CalculateTotalHoursAsync(workOrder.Id);
            var totalCost = (await CalculateMonthlyCostAsync(workOrder.Id))
                .Sum(cost => cost.MonthlyCost);
            var costPerMonth = (await CalculateMonthlyCostAsync(workOrder.Id))
                .Sum(cost => cost.MonthlyCost) / (await GetRelatedActivitiesSizeAsync(workOrder.Id));

            return new WorkOrderCost {
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
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task<int> CalculateTotalHoursAsync(int workOrderId) {
        return await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .SumAsync(x => x.Hours);
    }

    private async Task<int> GetRelatedEmployeesSizeAsync(int workOrderId) {
        return await _dbContext.Employees
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
        return await _dbContext.Employees
            .Where(em => _dbContext.EmployeeActivity
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

        var salaries = await _dbContext.Employees
            .Where(em => em.Salaries
                .Any(sal => sal.EmployeeId == em.Id) &&
                _dbContext.WorkTimeRecords
                    .Any(wtr => wtr.EmployeeId == em.Id))
            .Select(em => 
                new EmployeeSalary {
                    EmployeeId = em.Id,
                    StartDate = em.Salaries
                        .Single(sal => sal.EmployeeId == em.Id)
                        .StartDate,
                    FinishDate = em.Salaries
                        .Single(sal => sal.EmployeeId == em.Id)
                        .FinishDate,
                    Salary = em.Salaries
                        .Single(sal => sal.EmployeeId == em.Id)
                        .Salary,
                }
            )
            .ToListAsync();

        return workTimeRecords
            .Where(wtr => salaries
                .Any(sal => sal.EmployeeId == wtr.EmployeeId))
            .Select(activityCost =>
                new ActivityCost {
                    Id = activityCost.Id,
                    Name = activities
                        .Single(ac => ac.WorkOrderId == activityCost.WorkOrderId)
                        .Name,
                    StartDate = activities
                        .Single(ac => ac.WorkOrderId == activityCost.WorkOrderId)
                        .StartDate,
                    FinishDate = activities
                        .Single(ac => ac.WorkOrderId == activityCost.WorkOrderId)
                        .FinishDate,
                    Employees = _dbContext.Employees
                        .Where(em => em.Id == activityCost.EmployeeId)
                        .ToList(),
                    MonthlyCost = salaries
                        .Single(ems => ems.EmployeeId == activityCost.EmployeeId)
                        .Salary * activityCost.Hours
                }
            )
            .ToList();
    }
}