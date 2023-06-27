using System.Net;
using PlannerCRM.Shared.Models;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Client.Pages.Authentication;

public partial class Login
{
    [Inject] private CurrentUserInfoService CurrentUserInfoService { get; set; }
    [Inject] private LoginService LoginService { get; set; } 
    [Inject] private NavigationManager NavManager { get; set; }

    private EmployeeLoginDto _Model = new();   
    private CurrentEmployeeDto _CurrentEmployee { get; set; }
    private string _TypeField { get; set; } = InputType.PASSWORD.ToString().ToLower();
    private bool _IsCheckboxClicked { get; set; } = false;
    private string _Message { get; set; }
    private bool _IsError { get; set; }

    public async Task LoginOnValidInput() {
        var response = await LoginService.LoginAsync(_Model);

        if (response.IsSuccessStatusCode && _Model.Email != ADMIN_EMAIL) {
            _CurrentEmployee = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(_Model.Email);
        }
        
        if (response.IsSuccessStatusCode) {
            var role =  await CurrentUserInfoService.GetCurrentUserRoleAsync();

            foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
                if (possibleRole.ToString() == role) {
                    if (possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER) ||
                        possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER)) {
                        NavManager.NavigateTo($"{role.ToLower().Replace('_', '-')}/{_CurrentEmployee.Id}", true);
                    } else {
                        NavManager.NavigateTo($"{role.ToLower().Replace('_', '-')}", true);
                    }
                }
            }
        } else {
            _IsError = true;
            _Message = await response.Content.ReadAsStringAsync();
        }
    }

    public void SwitchShowPassword() {
         if (_IsCheckboxClicked) {
            _IsCheckboxClicked = false;
            _TypeField = InputType.TEXT.ToString().ToLower();
        } else {
            _IsCheckboxClicked = true;
            _TypeField = InputType.PASSWORD.ToString().ToLower();
        }
    }
}