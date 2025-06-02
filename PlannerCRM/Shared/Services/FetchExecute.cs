using PlannerCRM.Client.Services;
using PlannerCRM.Shared.Dtos;

namespace PlannerCRM.Shared.Services;

public partial class FetchExecute(FetchService fetch)
{
    private readonly FetchService _fetch = fetch;

    #region Activity

    public async Task<ResultDto> Activity_Insert(ActivityDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Update(ActivityDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Activity_Delete(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Get(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_List(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_LIST, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_Search(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_SEARCH, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_AssignActivity(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_ASSIGN_ACTIVITY, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_RemoveAssignedEmployee(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_REMOVE_ASSIGNED_EMPLOYEE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_FindAssociatedEmployeesByActivityid(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_FindAssociatedWorkOrdersByActivityid(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Activity_FindAssociatedWorkTimesWithinActivity(ActivityFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ACTIVITY_CONTROLLER, ApiUrl.ACTIVITY_FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY, filterDto, ApiType.Post);
    }

    #endregion

    #region Employee

    public async Task<ResultDto> Employee_Insert(EmployeeDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_Update(EmployeeDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Employee_Delete(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_Get(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_List(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_LIST, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_UpdateRole(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_UPDATE_ROLE, filterDto, ApiType.Put);
    }

    public async Task<ResultDto> Employee_Search(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_SEARCH, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_SearchByNameEmailPhoneForRecovery(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Employee_FindAssociatedActivitiesByEmployeeId(EmployeeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.EMPLOYEE_CONTROLLER, ApiUrl.EMPLOYEE_FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID, filterDto, ApiType.Post);
    }


    #endregion

    #region FirmClient

    public async Task<ResultDto> FirmClient_Insert(FirmClientDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Update(FirmClientDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> FirmClient_Delete(FirmClientFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Get(FirmClientFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_List(FirmClientFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_LIST, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_Search(FirmClientFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_SEARCH, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> FirmClient_FindAssociatedWorkOrdersByClientId(FirmClientFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.CLIENT_CONTROLLER, ApiUrl.CLIENT_FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID, filterDto, ApiType.Post);
    }


    #endregion

    #region Role

    public async Task<ResultDto> Role_Insert(RoleDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> Role_Update(RoleDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> Role_Delete(RoleFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_Get(RoleFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> Role_List(RoleFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.ROLE_CONTROLLER, ApiUrl.ROLE_LIST, filterDto, ApiType.Post);
    }


    #endregion

    #region WorkOrder

    public async Task<ResultDto> WorkOrder_Insert(WorkOrderDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Update(WorkOrderDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> WorkOrder_Delete(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Get(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_List(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_LIST, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_Search(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_SEARCH, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_FindAssociatedActivitiesByWorkOrderId(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_FindAssociatedWorkOrdersByClientId(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkOrder_FindAssociatedWorkOrdersByEmployeeId(WorkOrderFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKORDER_CONTROLLER, ApiUrl.WORKORDER_FIND_ASSOCIATED_WORKORDERS_BY_EMPLOYEEID, filterDto, ApiType.Post);
    }

    #endregion

    #region WorkTime

    public async Task<ResultDto> WorkTime_Insert(WorkTimeDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_INSERT, dto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_Update(WorkTimeDto dto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_UPDATE, dto, ApiType.Put);
    }

    public async Task<ResultDto> WorkTime_Delete(WorkTimeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_DELETE, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_Get(WorkTimeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_GET, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_List(WorkTimeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_LIST, filterDto, ApiType.Post);
    }

    public async Task<ResultDto> WorkTime_FindAssociatedWorkTimesByEmployeeId(WorkTimeFilterDto filterDto)
    {
        return await _fetch.ExecuteAsync(ApiUrl.WORKTIME_CONTROLLER, ApiUrl.WORKTIME_FIND_ASSOCIATED_WORKTIMES_BY_EMPLOYEE_ID, filterDto, ApiType.Post);
    }


    #endregion
}