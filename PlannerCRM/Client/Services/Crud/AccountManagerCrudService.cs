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

    public async Task<List<EmployeeViewDto>> GetAllEmployeesAsync() {
        try {
            var response = await _http.GetAsync("employee/get/all");
            var jsonObject = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<EmployeeViewDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<EmployeeViewDto>();
        }
    }

    public async Task<HttpResponseMessage> AddEmployeeAsync(EmployeeAddFormDto model) {
        try 
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("employee/add"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        } 
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error, nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(NotFound);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error, argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error, duplicateElemExc, duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> AddUserAsync(EmployeeAddFormDto model) {
        try 
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("applicationuser/add/user"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error, nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error, argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error, duplicateElemExc, duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    } 

    public async Task<HttpResponseMessage> UpdateEmployeeAsync(EmployeeEditFormDto model) {
       try
       {
         return await _http.SendAsync(new HttpRequestMessage() {
             Method = HttpMethod.Post,
             RequestUri = new Uri("employee/edit"),
             Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
         });
       }
       catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error, nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error, argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.Log(LogLevel.Error, keyNotFoundExc, keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return new HttpResponseMessage(NotFound);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(EmployeeEditFormDto model, string currentEmail) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("applicationuser/edit/user"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error, nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(NotFound);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error, argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.Log(LogLevel.Error, keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return new HttpResponseMessage(NotFound);
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.Log(LogLevel.Error, invalidOpExc.Message, invalidOpExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.StackTrace, exc.Message);
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
            _logger.Log(LogLevel.Error, exc.Message);
            return new EmployeeDeleteDto();
        }
    }

    public async Task<EmployeeEditFormDto> GetEmployeeForEditAsync(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"employee/get/for/edit/{employeeId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<EmployeeEditFormDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new EmployeeEditFormDto();
        }
    }

    public async Task<HttpResponseMessage> DeleteUserAsync(string currentEmail) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"applicationuser/delete/user/{currentEmail}")
            });
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.Log(LogLevel.Error, invalidOpExc, invalidOpExc.Message, invalidOpExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message, exc.StackTrace);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> DeleteEmployeeAsync(int employeeId) {
        try 
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"employee/delete/{employeeId}")
            });
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.Log(LogLevel.Error, invalidOpExc, invalidOpExc.Message, invalidOpExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message, exc.StackTrace);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }
}