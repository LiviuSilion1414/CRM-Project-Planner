namespace PlannerCRM.Server.Interfaces;

public interface IEmployeeRepository
{
    public Task ArchiveAsync(int employeeId);
    public Task RestoreAsync(int employeeId);
    public Task<EmployeeSelectDto> GetForRestoreAsync(int employeeId);
    public Task<EmployeeViewDto> GetForViewByIdAsync(int employeeId);
    public Task<EmployeeFormDto> GetForEditByIdAsync(int employeeId);
    public Task<EmployeeDeleteDto> GetForDeleteByIdAsync(int employeeId);
    public Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email);
    public Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset);
    public Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email);
    public Task<int> GetEmployeesSizeAsync();
}