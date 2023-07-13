using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using static System.Net.HttpStatusCode;

namespace PlannerCRM.Client.Services.Crud;

public class AccountManagerCrudService
{
    private readonly HttpClient _http;
    private readonly Logger<AccountManagerCrudService> _logger;

    public AccountManagerCrudService(
        HttpClient http,
        Logger<AccountManagerCrudService> logger) 
    {
        _http = http;
        _logger = logger;
    }

    public async Task<int> GetEmployeesSize() {
        try {
            var response = await _http.GetAsync("employee/get/size/");
            var jsonObject = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<int>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return 0;
        }
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployees(int skip = 0, int take = 5) {
        try {
            var response = await _http.GetAsync($"http://localhost:5032/employee/get/paginated/{skip}/{take}");
            var jsonObject = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<EmployeeViewDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new List<EmployeeViewDto>();
        }
    }

    public async Task<List<EmployeeViewDto>> GetAllEmployeesAsync() {
        try {
            var response = await _http.GetAsync("employee/get/all");
            var jsonObject = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<EmployeeViewDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new List<EmployeeViewDto>();
        }
    }

    public async Task<HttpResponseMessage> AddEmployeeAsync(EmployeeFormDto dto) {
        try 
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5032/employee/add"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        } 
        catch (NullReferenceException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(NotFound);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> AddUserAsync(EmployeeFormDto dto) {
        try 
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5032/applicationuser/add"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new HttpResponseMessage(ServiceUnavailable);
        }
    } 

    public async Task<HttpResponseMessage> UpdateEmployeeAsync(EmployeeFormDto dto) {
       try
       {
         return await _http.SendAsync(new HttpRequestMessage() {
             Method = HttpMethod.Put,
             RequestUri = new Uri("http://localhost:5032/employee/edit"),
             Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
         });
       }
       catch (NullReferenceException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (KeyNotFoundException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(NotFound);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(EmployeeFormDto dto) {
        try
        {
            return await _http.PutAsync("http://localhost:5032/applicationuser/edit", 
                new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json"));
        }
        catch (NullReferenceException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(NotFound);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (KeyNotFoundException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(NotFound);
        }
        catch (InvalidOperationException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    } 

    public async Task<EmployeeDeleteDto> GetEmployeeForDeleteAsync(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"employee/get/for/delete/{employeeId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<EmployeeDeleteDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new EmployeeDeleteDto();
        }
    }

    public async Task<EmployeeFormDto> GetEmployeeForEditAsync(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"employee/get/for/edit/{employeeId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<EmployeeFormDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<EmployeeViewDto> GetEmployeeForViewAsync(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"employee/get/for/view/{employeeId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<EmployeeViewDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new EmployeeViewDto();
        }
    }

    public async Task<HttpResponseMessage> DeleteUserAsync(string currentEmail) {
        try
        {
            return await _http.DeleteAsync($"http://localhost:5032/applicationuser/delete/{currentEmail}");
        }
        catch (InvalidOperationException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> DeleteEmployeeAsync(int employeeId) {
        try 
        {
            return await _http.DeleteAsync($"http://localhost:5032/employee/delete/{employeeId}");

        }
        catch (InvalidOperationException exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (Exception exc)
        {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }
}