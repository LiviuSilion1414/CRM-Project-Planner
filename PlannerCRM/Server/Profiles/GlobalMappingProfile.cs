namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>().MaxDepth(1);
        CreateMap<ActivityDto, Activity>().MaxDepth(1);

        CreateMap<ActivityWorkTime, ActivityWorkTimeDto>().MaxDepth(1);
        CreateMap<ActivityWorkTimeDto, ActivityWorkTime>().MaxDepth(1);

        CreateMap<ClientWorkOrder, ClientWorkOrderCostDto>().MaxDepth(1);
        CreateMap<ClientWorkOrderDto, ClientWorkOrderCost>().MaxDepth(1);

        CreateMap<ClientWorkOrder, ClientWorkOrderDto>().MaxDepth(1);
        CreateMap<ClientWorkOrderDto, ClientWorkOrder>().MaxDepth(1);

        CreateMap<EmployeeActivity, EmployeeActivityDto>().MaxDepth(1);
        CreateMap<EmployeeActivityDto, EmployeeActivity>().MaxDepth(1);

        CreateMap<Employee, EmployeeDto>().MaxDepth(1);
        CreateMap<EmployeeDto, Employee>().MaxDepth(1);

        CreateMap<EmployeeRole, EmployeeRoleDto>().MaxDepth(1);
        CreateMap<EmployeeRoleDto, EmployeeRole>().MaxDepth(1);

        CreateMap<EmployeeSalary, EmployeeSalaryDto>().MaxDepth(1);
        CreateMap<EmployeeSalaryDto, EmployeeSalary>().MaxDepth(1);

        CreateMap<EmployeeWorkTime, EmployeeWorkTimeDto>().MaxDepth(1);
        CreateMap<EmployeeWorkTimeDto, EmployeeWorkTime>().MaxDepth(1);

        CreateMap<FirmClient, FirmClientDto>().MaxDepth(1);
        CreateMap<FirmClientDto, FirmClient>().MaxDepth(1);

        CreateMap<Role, RoleDto>().MaxDepth(1);
        CreateMap<RoleDto, Role>().MaxDepth(1);

        CreateMap<Salary, SalaryDto>().MaxDepth(1);
        CreateMap<SalaryDto, Salary>().MaxDepth(1);

        CreateMap<WorkOrderActivity, WorkOrderActivityDto>().MaxDepth(1);
        CreateMap<WorkOrderActivityDto, WorkOrderActivity>().MaxDepth(1);

        CreateMap<WorkOrderCost, WorkOrderCostDto>().MaxDepth(1);
        CreateMap<WorkOrderCostDto, WorkOrderCost>().MaxDepth(1);

        CreateMap<WorkOrder, WorkOrderDto>().MaxDepth(1);
        CreateMap<WorkOrderDto, WorkOrder>().MaxDepth(1);

        CreateMap<WorkTime, WorkTimeDto>().MaxDepth(1);
        CreateMap<WorkTimeDto, WorkTime>().MaxDepth(1);

        CreateMap<EmployeeLoginDto, EmployeeLogin>().MaxDepth(1);
    }
}
