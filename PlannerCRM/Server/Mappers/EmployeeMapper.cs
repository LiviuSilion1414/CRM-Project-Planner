namespace PlannerCRM.Server.Mappers;

public static class EmployeeMapper
{
    private static AppDbContext _dbContext;

    public static void Configure(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public static CurrentEmployeeDto MapToCurrentEmployeeDto(this Employee employee)
    {
        return new CurrentEmployeeDto
        {
            Id = employee.Id,
            Email = employee.Email
        };
    }

    public static EmployeeFormDto MapToEmployeeFormDto(this Employee employee)
    {
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
            StartDateHourlyRate = default,
            FinishDateHourlyRate = default,
        };
    }

    public static EmployeeSalaryDto MapToEmployeeSalaryDto(this EmployeeSalary employeeSalary)
    {
        return new EmployeeSalaryDto
        {
            Id = employeeSalary.Id,
            EmployeeId = employeeSalary.EmployeeId,
            StartDate = employeeSalary.StartDate,
            FinishDate = employeeSalary.FinishDate,
            Salary = employeeSalary.Salary
        };
    }

    public static EmployeeDeleteDto MapToEmployeeDeleteDto(this Employee employee, int employeeId)
    {
        return new EmployeeDeleteDto
        {
            Id = employee.Id,
            FullName = $"{employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Role = employee.Role.ToString(),
            EmployeeActivities = []
        };
    }

    public static EmployeeActivityDto MapToEmployeeActivityDto(this EmployeeActivity employeeActivity)
    {
        return new EmployeeActivityDto
        {
            Id = employeeActivity.Id,
            EmployeeId = employeeActivity.EmployeeId,
            Employee = employeeActivity.Employee.MapToEmployeeSelectDto(),
            ActivityId = employeeActivity.ActivityId,
            Activity = employeeActivity.Activity.MapToActivitySelectDto(),
        };
    }

    public static EmployeeSelectDto MapToEmployeeSelectDto(this Employee employee)
    {
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

    public static EmployeeViewDto MapToEmployeeViewDto(this Employee employee)
    {
        return new EmployeeViewDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = $"{employee.FirstName} {employee.LastName}",
            BirthDay = employee.BirthDay,
            StartDate = employee.StartDate,
            Name = $"{employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Role = employee.Role,
            CurrentHourlyRate = employee.CurrentHourlyRate,
            IsDeleted = employee.IsDeleted,
            IsArchived = employee.IsArchived,
            EmployeeActivities = [], //todo: need to refactor queries
            EmployeeSalaries = []
        };
    }

    public static Employee MapToEmployee(this EmployeeFormDto dto)
    {
        return new Employee
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.Email,
            NormalizedEmail = dto.Email.ToUpper(),
            FullName = $"{dto.FirstName} {dto.LastName}",
            BirthDay = dto.BirthDay ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            Password = dto.Password,
            NumericCode = dto.NumericCode,
            Role = dto.Role ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            CurrentHourlyRate = dto.CurrentHourlyRate,
            Salaries = dto.EmployeeSalaries
                .Select(ems => ems.MapToEmployeeSalary(dto))
                .ToList()
        };
    }

    public static EmployeeSalary MapToEmployeeSalary(this EmployeeSalaryDto employeeSalaryDto, EmployeeFormDto dto)
    {
        return new EmployeeSalary
        {
            EmployeeId = dto.Id,
            StartDate = employeeSalaryDto.StartDate,
            FinishDate = employeeSalaryDto.FinishDate,
            Salary = employeeSalaryDto.Salary
        };
    }
}