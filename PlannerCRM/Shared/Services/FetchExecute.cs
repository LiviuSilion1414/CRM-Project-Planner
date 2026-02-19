using PlannerCRM.Shared.Services;
using PlannerCRM.Shared.Dtos;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Shared.Services;

public partial class FetchService
{
    #region Activity

    public async Task<ResultDto> Activity_Insert(ActivityDto dto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Update(ActivityDto dto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Activity_Delete(ActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Get(ActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_List(ActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region Employee

    public async Task<ResultDto> Employee_Insert(EmployeeDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_Update(EmployeeDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Employee_Delete(EmployeeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_Get(EmployeeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_List(EmployeeFilterDto filterDto)
    {
         return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_ListByEmployeeId(EmployeeFilterDto employeeFilterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.MENU_LIST_BY_EMPLOYEE_ID, employeeFilterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_ListByEmployeeId(EmployeeFilterDto employeeFilterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.MENU_LIST_BY_EMPLOYEE_ID, employeeFilterDto, ApiType.Post);
    }

    #endregion

    #region EmployeesRoles

    public async Task<ResultDto> EmployeesRoles_Insert(EmployeesRolesDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ROLES_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> EmployeesRoles_Update(EmployeesRolesFilterDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ROLES_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> EmployeesRoles_Delete(EmployeesRolesDto employeesRolesDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ROLES_CONTROLLER, ApiUrl.DELETE, employeesRolesDto, ApiType.Post);
    }

    public async Task<ResultDto> EmployeesRoles_Get(EmployeesRolesFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ROLES_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> EmployeesRoles_List(EmployeesRolesFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ROLES_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region EmployeeActivity

    public async Task<ResultDto> EmployeeActivity_Insert(EmployeeActivityDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ACTIVITIES_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> EmployeeActivity_Update(EmployeeActivityFilterDto dto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ACTIVITIES_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> EmployeeActivity_Delete(EmployeeActivityDto employeesRolesDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ACTIVITIES_CONTROLLER, ApiUrl.DELETE, employeesRolesDto, ApiType.Post);
    }

    public async Task<ResultDto> EmployeeActivity_Get(EmployeeActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ACTIVITIES_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> EmployeeActivity_List(EmployeeActivityFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.EMPLOYEES_ACTIVITIES_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region FirmClient

    public async Task<ResultDto> FirmClient_Insert(FirmClientDto dto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Update(FirmClientDto dto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> FirmClient_Delete(FirmClientFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Get(FirmClientFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_List(FirmClientFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region Role

    public async Task<ResultDto> Role_Insert(RoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Role_Update(RoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Role_Delete(RoleFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_Get(RoleFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_List(RoleFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }


    #endregion

    #region GroupRoles

    public async Task<ResultDto> GroupRoles_Insert(GroupRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.GROUP_ROLES_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> GroupRoles_Update(GroupRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.GROUP_ROLES_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> GroupRoles_Delete(GroupRolesFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.GROUP_ROLES_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> GroupRoles_Get(GroupRolesFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.GROUP_ROLES_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> GroupRoles_List(GroupRolesFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.GROUP_ROLES_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }


    #endregion

    #region Menu Role

    public async Task<ResultDto> MenuRoles_Insert(MenuRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> MenuRoles_Update(MenuRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> MenuRoles_Delete(MenuRoleDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.DELETE, dto, ApiType.Post);
    }

    public async Task<ResultDto> MenuRoles_Get(MenuRoleFilterDto menuFilterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.GET, menuFilterDto, ApiType.Post);
    }

    public async Task<ResultDto> MenuRoles_List(MenuRoleFilterDto menuFilterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_ROLE_CONTROLLER, ApiUrl.LIST, menuFilterDto, ApiType.Post);
    }

    #endregion

    #region Menu

    public async Task<ResultDto> Menu_Insert(MenuDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_Update(MenuDto dto)
    {
        return await ExecuteAsync(ApiUrl.MENU_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Menu_Delete(MenuFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_Get(MenuFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Menu_List(MenuFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.MENU_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }


    #endregion

    #region WorkOrder

    public async Task<ResultDto> WorkOrder_Insert(WorkOrderDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Update(WorkOrderDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> WorkOrder_Delete(WorkOrderFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Get(WorkOrderFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_List(WorkOrderFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region WorkTime

    public async Task<ResultDto> WorkTime_Insert(WorkTimeDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_Update(WorkTimeDto dto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> WorkTime_Delete(WorkTimeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_Get(WorkTimeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_List(WorkTimeFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region Settings

    public async Task<ResultDto> Settings_Insert(SettingDto dto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Settings_Update(SettingDto dto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Settings_Delete(SettingFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Settings_Get(SettingFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Settings_List(SettingFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SETTINGS_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion

    #region SystemLog

    public async Task<ResultDto> SystemLog_Insert(SystemLogDto dto)
    {
        return await ExecuteAsync(ApiUrl.SYSTEMLOG_CONTROLLER, ApiUrl.INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> SystemLog_Update(SystemLogDto dto)
    {
        return await ExecuteAsync(ApiUrl.SYSTEMLOG_CONTROLLER, ApiUrl.UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> SystemLog_Delete(SystemLogFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SYSTEMLOG_CONTROLLER, ApiUrl.DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> SystemLog_Get(SystemLogFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SYSTEMLOG_CONTROLLER, ApiUrl.GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> SystemLog_List(SystemLogFilterDto filterDto)
    {
        return await ExecuteAsync(ApiUrl.SYSTEMLOG_CONTROLLER, ApiUrl.LIST, filterDto, ApiType.Post);
    }

    #endregion
}