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
            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == workOrderId)
                    ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
    
            var isAnyInvoice = await _dbContext.WorkOrderCosts
                .AnyAsync(inv => inv.WorkOrderId == workOrderId);
    
            if (isAnyInvoice && workOrder is null) {
                throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
            }

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
                    ClientId = wo.ClientId,
                    //Client = new ClientViewDto {
                    //    Id = wo.Client.Id,
                    //    Name = wo.Client.Name,
                    //    VatNumber = wo.Client.VatNumber,
                    //    WorkOrderId = wo.Client.WorkOrderId, 
                    //}
                }
            )
            .ToListAsync();
    }

    public async Task<WorkOrderCostDto> GetWorkOrderCosts(int workOrderId) {
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
                    //Employees = inv.Employees,
                    //Activities = inv.Activities,
                    //MonthlyActivityCosts = inv.MonthlyActivityCosts,
                    TotalEmployees = inv.TotalEmployees,
                    TotalActivities = inv.TotalActivities,
                    TotalHours = inv.TotalHours,
                    TotalCost = inv.TotalCost,
                    CostPerMonth = inv.CostPerMonth
                }
            )
            .SingleOrDefaultAsync();
    }

    private async Task<WorkOrderCost> ExecuteCalculationsAsync(WorkOrder workOrder) {
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
            Id = workOrder.Id,
            WorkOrderId = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            TotalTime = workOrder.FinishDate - workOrder.StartDate,
            IsCompleted = workOrder.IsCompleted,
            IsDeleted = workOrder.IsDeleted,
            IssuedDate = DateTime.Now,
            ClientId = workOrder.ClientId,
            //Employees = employees,
            //Activities = activities,
            //MonthlyActivityCosts = monthlyActivityCosts,
            TotalEmployees = totalEmployees,
            TotalActivities = totalActivities,
            TotalHours = totalHours,
            TotalCost = totalCost,
            CostPerMonth = costPerMonth
        };
    }

    private async Task<int> CalculateTotalHoursAsync(int workOrderId) {
        return await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .SumAsync(x => x.Hours);
    }

    private async Task<int> GetRelatedEmployeesSizeAsync(int workOrderId) {
        return await _dbContext.Employees
            .Where(em => _dbContext.EmployeeActivity
                .Any(ea => em.Id == ea.EmployeeId))
            .CountAsync();
    }

    private async Task<int> GetRelatedActivitiesSizeAsync(int workOrderId) {
        return await _dbContext.Activities
            .Where(ac => _dbContext.EmployeeActivity
                .Any(ea => ac.Id == ea.ActivityId))
            .CountAsync();
    }

    private async Task<List<EmployeeViewDto>> GetAllRelatedEmployeesAsync(int workOrderId) {
        return await _dbContext.Employees
            .AsNoTracking()
            .Where(em => _dbContext.EmployeeActivity
                .Any(ea => em.Id == ea.EmployeeId))
            .Select(em =>
                new EmployeeViewDto {
                    Id = em.Id,
                    FirstName = em.FirstName,
                    LastName = em.LastName,
                    FullName = $"{em.FirstName} {em.LastName}",
                    BirthDay = em.BirthDay,
                    StartDate = em.StartDate,
                    NumericCode = em.NumericCode,
                    Password = em.Password,
                    StartDateHourlyRate = em.Salaries.Single().StartDate,
                    FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                    Email = em.Email,
                    IsDeleted = em.IsDeleted,
                    IsArchived = em.IsArchived,
                    Role = em.Role, 
                    CurrentHourlyRate = em.CurrentHourlyRate,
                    EmployeeSalaries = em.Salaries
                        .Select( ems => new EmployeeSalaryDto {
                            Id = ems.Id,
                            EmployeeId = ems.Id,
                            StartDate = ems.StartDate,
                            FinishDate = ems.StartDate,
                            Salary = ems.Salary})
                        .ToList(),
                    EmployeeActivities = em.EmployeeActivity
                        .Select( ea =>
                            new EmployeeActivityDto {
                                EmployeeId = ea.Id,
                                Employee = new EmployeeSelectDto {
                                    Email = ea.Employee.Email,
                                    FirstName = ea.Employee.FirstName,
                                    LastName = ea.Employee.LastName,
                                    Role = ea.Employee.Role,
                                    CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                },
                                ActivityId = ea.ActivityId,
                                Activity = new ActivitySelectDto {
                                    Id = ea.Activity.Id,
                                    Name = ea.Activity.Name,
                                    StartDate = ea.Activity.StartDate,
                                    FinishDate = ea.Activity.FinishDate,
                                    WorkOrderId = ea.Activity.WorkOrderId
                                }
                            })
                        .ToList()
            })
            .ToListAsync();
    }

    private async Task<List<ActivityViewDto>> GetAllRelatedActivitiesAsync(int workOrderId) {
        return await _dbContext.Activities
            .AsNoTracking()
            .Where(ac => _dbContext.EmployeeActivity
                .Any(ea => ac.Id == ea.ActivityId))
            .Select(ac => new ActivityViewDto {
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId
            })
            .ToListAsync();
    }

    private async Task<List<ActivityCostDto>> CalculateMonthlyCostAsync(int workOrderId) {
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
                    Id = em.Id,
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
                new ActivityCostDto {
                    Id = activityCost.Id,
                    Name = activities
                        .Single(ac => ac.Id == activityCost.ActivityId)
                        .Name,
                    StartDate = activities
                        .Single(ac => ac.Id == activityCost.ActivityId)
                        .StartDate,
                    FinishDate = activities
                        .Single(ac => ac.Id == activityCost.ActivityId)
                        .FinishDate,
                    Employees = _dbContext.Employees
                        .Where(em => em.Id == activityCost.EmployeeId)
                        .Select(em => 
                            new EmployeeViewDto {
                                Id = em.Id,
                                FirstName = em.FirstName,
                                LastName = em.LastName,
                                FullName = $"{em.FirstName} {em.LastName}",
                                BirthDay = em.BirthDay,
                                StartDate = em.StartDate,
                                NumericCode = em.NumericCode,
                                Password = em.Password,
                                StartDateHourlyRate = em.Salaries.Single().StartDate,
                                FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                                Email = em.Email,
                                IsDeleted = em.IsDeleted,
                                IsArchived = em.IsArchived,
                                Role = em.Role, 
                                CurrentHourlyRate = em.CurrentHourlyRate
                            })
                        .ToList(),
                    MonthlyCost = salaries
                        .Single(ems => ems.EmployeeId == activityCost.EmployeeId)
                        .Salary * activityCost.Hours
                }
            )
            .ToList();
    }
}