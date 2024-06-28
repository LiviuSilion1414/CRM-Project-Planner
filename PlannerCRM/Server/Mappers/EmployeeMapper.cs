namespace PlannerCRM.Server.Mappers;

public static class EmployeeMapper
{
    public static CurrentEmployeeDto MapToCurrentEmployeeDto(this Employee employee) {
        return new CurrentEmployeeDto
        {
            Id = employee.Id,
            Email = employee.Email
        };
    }

    public static EmployeeFormDto MapToEmployeeFormDto(this Employee employee, AppDbContext context) {
        return new EmployeeFormDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            OldEmail = employee.Email,
            BirthDay = employee.BirthDay,
            StartDate = employee.StartDate,
            Role = employee.Role,
            NumericCode = employee.NumericCode,
            Password = employee.Password,
            CurrentHourlyRate = employee.CurrentHourlyRate,
            IsDeleted = employee.IsDeleted,
            StartDateHourlyRate = employee.Salaries.Single().StartDate,
            FinishDateHourlyRate = employee.Salaries.Single().FinishDate,
            EmployeeSalaries = employee.Salaries
                    .Where(ems => context.Employees
                        .Any(employee => employee.Id == ems.EmployeeId))
                    .Select(ems => new EmployeeSalaryDto
                    {
                        Id = ems.Id,
                        EmployeeId = employee.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary
                    })
                    .ToList()
        };
    }

    public static EmployeeDeleteDto MapToEmployeeDeleteDto(this Employee employee, int employeeId, AppDbContext context) {
        return new EmployeeDeleteDto
        {
            Id = employee.Id,
            FullName = $"{employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Role = employee.Role
                .ToString()
                .Replace('_', ' '),
            EmployeeActivities = context.EmployeeActivity
                .Where(ea => ea.EmployeeId == employeeId)
                .Select(ea => new EmployeeActivityDto
                {
                    Id = ea.Id,
                    EmployeeId = ea.EmployeeId,
                    Employee = context.Employees
                        .Where(e => e.Id == ea.EmployeeId)
                        .Select(_ => new EmployeeSelectDto
                        {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FullName = ea.Employee.FullName,
                        })
                        .Single(),
                    ActivityId = ea.ActivityId,
                    Activity = context.Activities
                        .Where(ac => ac.Id == ea.ActivityId)
                        .Select(_ => new ActivitySelectDto
                        {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            WorkOrderId = ea.Activity.WorkOrderId,
                        })
                        .Single()
                })
                .ToList()

        };
    }

    public static EmployeeSelectDto MapToEmployeeSelectDto(this Employee employee) {
        return new EmployeeSelectDto
        {
            Id = employee.Id,
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = employee.FullName,
            Role = employee.Role
        };
    }

    public static EmployeeViewDto MapToEmployeeViewDto(this Employee employee, AppDbContext context) {
        return new EmployeeViewDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = $"{employee.FirstName} {employee.LastName}",
            BirthDay = employee.BirthDay,
            StartDate = employee.StartDate,
            Email = employee.Email,
            Role = employee.Role,
            CurrentHourlyRate = employee.CurrentHourlyRate,
            IsDeleted = employee.IsDeleted,
            IsArchived = employee.IsArchived,
            EmployeeActivities = employee.EmployeeActivity
                .Select(ea => new EmployeeActivityDto
                {
                    Id = ea.ActivityId,
                    EmployeeId = employee.Id,
                    Employee = context.Employees
                        .Select(employee => new EmployeeSelectDto
                        {
                            Id = employee.Id,
                            Email = employee.Email,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Role = employee.Role
                        })
                        .Single(employee => employee.Id == ea.EmployeeId),
                    ActivityId = ea.ActivityId,
                    Activity = context.Activities
                        .Select(ac => new ActivitySelectDto
                        {
                            Id = ac.Id,
                            Name = ac.Name,
                            StartDate = ac.StartDate,
                            FinishDate = ac.FinishDate,
                            WorkOrderId = ac.WorkOrderId
                        })
                    .Single(ac => ac.Id == ea.ActivityId),
                }).ToList(),
            EmployeeSalaries = employee.Salaries
                .Select(ems => new EmployeeSalaryDto
                {
                    Id = ems.Id,
                    EmployeeId = ems.Id,
                    StartDate = ems.StartDate,
                    FinishDate = ems.StartDate,
                    Salary = ems.Salary
                })
                .ToList(),
        };
    }

    public static Employee MapToEmployee(this EmployeeFormDto dto) {
        return new Employee
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FullName = $"{dto.FirstName} {dto.LastName}",
            Username = dto.Email,
            BirthDay = dto.BirthDay ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            Password = dto.Password,
            NumericCode = dto.NumericCode,
            Role = dto.Role ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            CurrentHourlyRate = dto.CurrentHourlyRate,
            Salaries = dto.EmployeeSalaries
                .Select(ems =>
                    new EmployeeSalary
                    {
                        StartDate = ems.StartDate,
                        FinishDate = ems.FinishDate,
                        Salary = ems.Salary
                    }
                )
                .ToList()
        };
    }

    public static EmployeeProfilePicture MapToEmployeeProfilePicture(this EmployeeFormDto dto, Employee existingEmployee) {
        return new EmployeeProfilePicture
        {
            ImageType = dto.ProfilePicture.ImageType,
            Thumbnail = dto.ProfilePicture.Thumbnail,
            EmployeeId = existingEmployee.Id,
            EmployeeInfo = existingEmployee
        };
    }

    public static EmployeeSalary MapToEmployeeSalary(this EmployeeSalaryDto employeeSalaryDto, EmployeeFormDto dto) {
        return new EmployeeSalary
        {
            EmployeeId = dto.Id,
            StartDate = employeeSalaryDto.StartDate,
            FinishDate = employeeSalaryDto.FinishDate,
            Salary = employeeSalaryDto.Salary
        };
    }
}