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

    public async Task<Dictionary<WorkOrderInvoiceDto, WorkOrderCostDto>> IssueInvoiceAsync(int workOrderId) {
        try {
            var exists = await _dbContext.WorkOrders
                .AnyAsync(wo => wo.Id == workOrderId);

            if (!exists) {
                throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
            }

            var invoiceInfo = new Dictionary<WorkOrderInvoiceDto, WorkOrderCostDto>();

            var workOrderCost = new WorkOrderCostDto {
                Id = workOrderId,
                ClientId = _dbContext.Clients
                        .Single(cl => cl.WorkOrderId == workOrderId)
                        .Id,
                WorkOrderId = workOrderId,
                Employees = await GetAllRelatedEmployeesAsync(workOrderId),
                Activities = await GetAllRelatedActivitiesAsync(workOrderId),
                MonthlyActivityCosts = await CalculateMonthlyCostAsync(workOrderId),
                TotalEmployees = await GetRelatedEmployeesSizeAsync(workOrderId),
                TotalActivities = await GetRelatedActivitiesSizeAsync(workOrderId),
                TotalHours = await CalculateTotalHoursAsync(workOrderId),
                TotalCost = (await CalculateMonthlyCostAsync(workOrderId))
                                    .Sum(cost => cost.MonthlyCost),
                CostPerMonth = (await CalculateMonthlyCostAsync(workOrderId))
                                .Sum(cost => cost.MonthlyCost) / (await GetRelatedActivitiesSizeAsync(workOrderId)),
            };

            var workOrderInvoice = new WorkOrderInvoice {
                IssuedDate = DateTime.Now,
                ClientId = workOrderCost.ClientId,
                WorkOrderId = workOrderCost.Id,
                TotalAmount = workOrderCost.TotalCost
            };

            await _dbContext.Invoices.AddAsync(
                new WorkOrderInvoice {
                    IssuedDate = DateTime.Now,
                    ClientId = workOrderCost.ClientId,
                    WorkOrderId = workOrderCost.Id,
                    TotalAmount = workOrderCost.TotalCost
                }
            );

            if (await _dbContext.SaveChangesAsync() == 0) {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }

            var invoice = await GetInvoiceInfoAsync(workOrderId);

            invoiceInfo.Add(invoice, workOrderCost);

            return invoiceInfo;
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            throw;
        } 
    }

    private async Task<WorkOrderInvoiceDto> GetInvoiceInfoAsync(int workOrderId) {
        return await _dbContext.Invoices
            .Select(inv => 
                new WorkOrderInvoiceDto {
                    Id = inv.Id,
                    ClientId = inv.ClientId,
                    IssuedDate = inv.IssuedDate,
                    TotalAmount = inv.TotalAmount,
                    WorkOrderId = inv.WorkOrderId
                })
            .SingleAsync(inv => inv.WorkOrderId == workOrderId);
    }
    
    public async Task<List<WorkOrderCostDto>> GetPaginatedWorkOrdersCostsAsync(int limit, int offset) {
        var paginatedWorkOrders = new List<WorkOrderCostDto>();

        await _dbContext.WorkOrders
            .Skip(limit)
            .Take(offset)
            .ForEachAsync(async wo => 
                {
                    paginatedWorkOrders
                        .Add(new WorkOrderCostDto {
                            Id = wo.Id,
                            ClientId = _dbContext.Clients
                                .Single(cl => cl.WorkOrderId == wo.Id)
                                .Id,
                            WorkOrderId = wo.Id,
                            Employees = await GetAllRelatedEmployeesAsync(wo.Id),
                            Activities = await GetAllRelatedActivitiesAsync(wo.Id),
                            MonthlyActivityCosts = await CalculateMonthlyCostAsync(wo.Id),
                            TotalEmployees = await GetRelatedEmployeesSizeAsync(wo.Id),
                            TotalActivities = await GetRelatedActivitiesSizeAsync(wo.Id),
                            TotalHours = await CalculateTotalHoursAsync(wo.Id),
                            TotalCost = (await CalculateMonthlyCostAsync(wo.Id))
                                .Sum(cost => cost.MonthlyCost),
                            CostPerMonth = (await CalculateMonthlyCostAsync(wo.Id))
                                .Sum(cost => cost.MonthlyCost) / (await GetRelatedActivitiesSizeAsync(wo.Id)),
                        });
                }
            );
        
        return paginatedWorkOrders;
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
                                    Id = ea.Employee.Id,
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
            .Where(ac => _dbContext.EmployeeActivity
                .Any(ea => ac.Id == ea.ActivityId))
            .Select(ac => new ActivityViewDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => 
                        new EmployeeActivityDto {
                            Id = ea.Id,
                            EmployeeId = ea.EmployeeId,
                            Employee = _dbContext.Employees
                                .Select(em => new EmployeeSelectDto {
                                    Id = ea.EmployeeId,
                                    Email = ea.Employee.Email,
                                    FirstName = ea.Employee.FirstName,
                                    LastName = ea.Employee.LastName,
                                    FullName = ea.Employee.FullName,
                                    Role = ea.Employee.Role,
                                    CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                    EmployeeSalaries = ea.Employee.Salaries
                                        .Select(sal => new EmployeeSalaryDto {
                                            Id = sal.Id,
                                            EmployeeId = ea.EmployeeId,
                                            StartDate = sal.StartDate,
                                            FinishDate = sal.FinishDate,
                                            Salary = sal.Salary,
                                        }).ToList()
                        }
                    )
                    .Single(em => em.Id == ea.EmployeeId), 
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(a => 
                                new ActivitySelectDto {
                                    Id = ea.ActivityId,
                                    Name = ea.Activity.Name,
                                    StartDate = ea.Activity.StartDate,
                                    FinishDate = ea.Activity.FinishDate,
                                    WorkOrderId = ea.Activity.WorkOrderId
                                }
                            )
                            .Single(ac => ac.Id == ea.ActivityId) 
                    }).ToHashSet()
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
                .Any(sal => workTimeRecords
                    .Any(wtr => wtr.EmployeeId == sal.EmployeeId)
                )
            )
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
                                                Id = ea.Employee.Id,
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
                        .ToList(),
                    MonthlyCost = salaries
                        .Single(ems => ems.EmployeeId == activityCost.EmployeeId)
                        .Salary * activityCost.Hours
                }
            )
            .ToList();
    }
}