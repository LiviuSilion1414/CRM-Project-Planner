namespace PlannerCRM.Server.Mappers;

public static class ActivityMapper
{
    public static Activity MapToActivity(this ActivityFormDto dto) {
        return new Activity
        {
            Id = dto.Id,
            Name = dto.Name,
            StartDate = dto.StartDate 
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            FinishDate = dto.FinishDate 
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            WorkOrderId = dto.WorkOrderId 
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            EmployeeActivity = dto.EmployeeActivity
                .Select(ea => new EmployeeActivity
                {
                    Id = ea.Id,
                    EmployeeId = ea.EmployeeId,
                    ActivityId = dto.Id
                })
                .ToHashSet()
        };
    }

    public static ActivityFormDto MapToActivityFormDto(this Activity activity, AppDbContext context) {
        return new ActivityFormDto {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            //ClientName = context.Clients
            //    .Single(cl => cl.WorkOrdersIds.Any(id => id == activity.WorkOrderId)
            //    .Name,
            EmployeeActivity = new(),
            ViewEmployeeActivity = activity.EmployeeActivity
                .Select(ea => new EmployeeActivityDto
                {
                    Id = ea.Id,
                    EmployeeId = ea.EmployeeId,
                    Employee = context.Employees
                        .Select(em => new EmployeeSelectDto
                        {
                            Id = ea.EmployeeId,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            FullName = ea.Employee.FullName,
                            Role = ea.Employee.Role,
                            CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                            EmployeeSalaries = ea.Employee.Salaries
                                .Select(sal => new EmployeeSalaryDto
                                {
                                    Id = sal.Id,
                                    EmployeeId = ea.EmployeeId,
                                    StartDate = sal.StartDate,
                                    FinishDate = sal.FinishDate,
                                    Salary = sal.Salary,
                                }).ToList()
                        })
                        .Single(em => em.Id == ea.EmployeeId),
                    ActivityId = ea.ActivityId,
                    Activity = context.Activities
                        .Select(a => new ActivitySelectDto
                        {
                            Id = ea.ActivityId,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        })
                        .Single(activity => activity.Id == ea.ActivityId)
                })
                .ToHashSet()
        };
    }

    public static EmployeeActivity MapToEmployeeActivity(this EmployeeActivityDto dto, int activityId) {
        return new EmployeeActivity
        {
            Id = dto.Id,
            EmployeeId = dto.EmployeeId,
            ActivityId = activityId
        };
    }

    public static ActivityViewDto MapToActivityViewDto(this Activity activity, AppDbContext context) {
        return new ActivityViewDto
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            EmployeeActivity = activity.EmployeeActivity
                .Select(ea => new EmployeeActivityDto
                {
                    Id = ea.Id,
                    EmployeeId = ea.EmployeeId,
                    Employee = context.Employees
                        .Select(em => new EmployeeSelectDto
                        {
                            Id = ea.EmployeeId,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            FullName = ea.Employee.FullName,
                            Role = ea.Employee.Role,
                            CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                            EmployeeSalaries = ea.Employee.Salaries
                                .Select(sal => new EmployeeSalaryDto
                                {
                                    Id = sal.Id,
                                    EmployeeId = ea.EmployeeId,
                                    StartDate = sal.StartDate,
                                    FinishDate = sal.FinishDate,
                                    Salary = sal.Salary,
                                }).ToList()
                        })
                        .Single(em => em.Id == ea.EmployeeId),
                    ActivityId = ea.ActivityId,
                    Activity = context.Activities
                        .Select(a => new ActivitySelectDto
                        {
                            Id = ea.ActivityId,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        })
                        .Single(activity => activity.Id == ea.ActivityId)
                }).ToHashSet()
        };
    }

    public static ActivityDeleteDto MapToActivityDeleteDto(this Activity activity, AppDbContext context, int activityId) {
        return new ActivityDeleteDto
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            Employees = context.EmployeeActivity
                    .Where(ea => ea.ActivityId == activityId)
                    .Select(ea => new EmployeeSelectDto()
                    {
                        Id = ea.EmployeeId,
                        Email = ea.Employee.Email,
                        FirstName = ea.Employee.FirstName,
                        LastName = ea.Employee.LastName,
                        FullName = ea.Employee.FullName,
                        Role = ea.Employee.Role,
                        CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                        EmployeeSalaries = ea.Employee.Salaries
                            .Select(sal => new EmployeeSalaryDto
                            {
                                Id = sal.Id,
                                EmployeeId = ea.EmployeeId,
                                StartDate = sal.StartDate,
                                FinishDate = sal.FinishDate,
                                Salary = sal.Salary,
                            }).ToList(),
                        EmployeeActivities = activity.EmployeeActivity
                            .Select(ea => new EmployeeActivityDto
                            {
                                Id = ea.Id,
                                EmployeeId = ea.EmployeeId,
                                Employee = context.Employees
                                    .Select(em => new EmployeeSelectDto
                                    {
                                        Id = ea.EmployeeId,
                                        Email = ea.Employee.Email,
                                        FirstName = ea.Employee.FirstName,
                                        LastName = ea.Employee.LastName,
                                        FullName = ea.Employee.FullName,
                                        Role = ea.Employee.Role,
                                        CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                        EmployeeSalaries = ea.Employee.Salaries
                                            .Select(sal => new EmployeeSalaryDto
                                            {
                                                Id = sal.Id,
                                                EmployeeId = ea.EmployeeId,
                                                StartDate = sal.StartDate,
                                                FinishDate = sal.FinishDate,
                                                Salary = sal.Salary,
                                            }).ToList()
                                    })
                                    .Single(em => em.Id == ea.EmployeeId),
                                ActivityId = ea.ActivityId,
                                Activity = context.Activities
                                    .Select(a => new ActivitySelectDto
                                    {
                                        Id = ea.ActivityId,
                                        Name = ea.Activity.Name,
                                        StartDate = ea.Activity.StartDate,
                                        FinishDate = ea.Activity.FinishDate,
                                        WorkOrderId = ea.Activity.WorkOrderId
                                    })
                                    .Single(activity => activity.Id == ea.ActivityId)
                            }).ToList()
                    })
                    .ToHashSet()
        };
    }

}