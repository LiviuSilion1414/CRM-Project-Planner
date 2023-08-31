namespace PlannerCRM.Client.Services.Crud;

public class AccountManagerCrudService
{
    private readonly HttpClient _http;
    private readonly ILogger<AccountManagerCrudService> _logger;

    public AccountManagerCrudService(HttpClient http, Logger<AccountManagerCrudService> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<int> GetEmployeesSize() {
        try {
            return await _http
                .GetFromJsonAsync<int>("api/employee/get/size/");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployees(int limit = 0, int offset = 5) {
        try {
            return await _http
                .GetFromJsonAsync<List<EmployeeViewDto>>($"api/employee/get/paginated/{limit}/{offset}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<HttpResponseMessage> AddEmployeeAsync(EmployeeFormDto dto) {
        try {
            return await _http
                .PostAsJsonAsync("api/employee/add", dto);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> AddUserAsync(EmployeeFormDto dto) {
        try {
            return await _http
                .PostAsJsonAsync("api/applicationuser/add", dto);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    } 

    public async Task<HttpResponseMessage> UpdateEmployeeAsync(EmployeeFormDto dto) {
        try {
            return await _http
                .PutAsJsonAsync("api/employee/edit", dto);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(EmployeeFormDto dto) {
        try {
            return await _http
                .PutAsJsonAsync("api/applicationuser/edit", dto);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    } 

    public async Task<EmployeeDeleteDto> GetEmployeeForDeleteByIdAsync(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<EmployeeDeleteDto>($"api/employee/get/for/delete/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<EmployeeFormDto> GetEmployeeForEditByIdAsync(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<EmployeeFormDto>($"api/employee/get/for/edit/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<EmployeeViewDto> GetEmployeeForViewByIdAsync(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<EmployeeViewDto>($"api/employee/get/for/view/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
    
    public async Task<EmployeeSelectDto> GetEmployeeForRestoreAsync(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<EmployeeSelectDto>($"api/employee/get/for/restore/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<HttpResponseMessage> DeleteUserAsync(string currentEmail) {
        try {
            return await _http
                .DeleteAsync($"api/applicationuser/delete/{currentEmail}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteEmployeeAsync(int employeeId) {
        try {
            return await _http
                .DeleteAsync($"api/employee/delete/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> ArchiveEmployeeAsync(int employeeId) {
        try {
            return await _http
                .GetAsync($"api/employee/archive/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> RestoreEmployeeAsync(int employeeId) {
        try {
            return await _http
                .GetAsync("api/employee/restore/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
            
            return new() { ReasonPhrase = exc.StackTrace };
        }
    }
}