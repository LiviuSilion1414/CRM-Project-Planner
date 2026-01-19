using PlannerCRM.Shared.Services;
using PlannerCRM.Shared.Dtos;

namespace PlannerCRM.Shared.Services;

public partial class FetchService
{
    #region Activity

    public async Task<ResultDto> Activity_Insert(ActivityDto dto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Update(ActivityDto dto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Activity_Delete(ActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Get(ActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_List(ActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region Employee

    public async Task<ResultDto> Employee_Insert(EmployeeDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_Update(EmployeeDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Employee_Delete(EmployeeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_Get(EmployeeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_List(EmployeeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region FirmClient

    public async Task<ResultDto> FirmClient_Insert(FirmClientDto dto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Update(FirmClientDto dto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> FirmClient_Delete(FirmClientFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Get(FirmClientFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_List(FirmClientFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region Role

    public async Task<ResultDto> Role_Insert(RoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Role_Update(RoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Role_Delete(RoleFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_Get(RoleFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_List(RoleFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_LIST, filterDto, ApiType.Post);
    }


    #endregion

    #region Menu Role

    public async Task<ResultDto> Menu_Role_Insert(MenuRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.MENU_ROLE_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_Role_Update(MenuRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.MENU_ROLE_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Menu_Role_Delete(MenuRoleFilterDto menuFilterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.MENU_ROLE_DELETE, menuFilterDto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_Role_Get(MenuRoleFilterDto menuFilterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.MENU_ROLE_GET, menuFilterDto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_Role_List(MenuRoleFilterDto menuFilterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.MENU_ROLE_LIST, menuFilterDto, ApiType.Post);
    }

    #endregion

    #region WorkOrder

    public async Task<ResultDto> WorkOrder_Insert(WorkOrderDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Update(WorkOrderDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> WorkOrder_Delete(WorkOrderFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Get(WorkOrderFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_List(WorkOrderFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region WorkTime

    public async Task<ResultDto> WorkTime_Insert(WorkTimeDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_Update(WorkTimeDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> WorkTime_Delete(WorkTimeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_Get(WorkTimeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_List(WorkTimeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region Settings

    public async Task<ResultDto> Settings_Insert(SettingDto dto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.SETTINGS_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Settings_Update(SettingDto dto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.SETTINGS_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Settings_Delete(SettingFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.SETTINGS_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Settings_Get(SettingFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.SETTINGS_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Settings_List(SettingFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.SETTINGS_LIST, filterDto, ApiType.Post);
    }

    #endregion
}