namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>().PreserveReferences().ReverseMap();
        //CreateMap<ActivityDto, Activity>().PreserveReferences().ReverseMap();

        CreateMap<ActivityWorkTime, ActivityWorkTimeDto>().PreserveReferences().ReverseMap();
        //CreateMap<ActivityWorkTimeDto, ActivityWorkTime>().PreserveReferences().ReverseMap();

        CreateMap<ClientWorkOrder, ClientWorkOrderCostDto>().PreserveReferences().ReverseMap();
        //CreateMap<ClientWorkOrderDto, ClientWorkOrderCost>().PreserveReferences().ReverseMap();

        CreateMap<ClientWorkOrder, ClientWorkOrderDto>().PreserveReferences().ReverseMap();
        //CreateMap<ClientWorkOrderDto, ClientWorkOrder>().PreserveReferences().ReverseMap();

        CreateMap<EmployeeActivity, EmployeeActivityDto>().PreserveReferences().ReverseMap();
        //CreateMap<EmployeeActivityDto, EmployeeActivity>().PreserveReferences().ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(x => x.PhoneNumber, y => y.MapFrom(z => z.Phone))
            .ForMember(x => x.Password, y => y.MapFrom(z => z.PasswordHash))
            .ForMember(x => x.EmployeeRoles, y => y.MapFrom(z => z.EmployeeRoles))
            .PreserveReferences()
            .ReverseMap();

        //CreateMap<EmployeeDto, Employee>()
        //    .ForMember(x => x.Phone, y => y.MapFrom(z => z.PhoneNumber))
        //    .ForMember(x => x.PasswordHash, y => y.MapFrom(z => z.Password))
        //    .PreserveReferences()
        //    .ReverseMap();

        CreateMap<EmployeeRole, EmployeeRoleDto>().PreserveReferences().ReverseMap();
        //CreateMap<EmployeeRoleDto, EmployeeRole>().PreserveReferences().ReverseMap();

        CreateMap<EmployeeSalary, EmployeeSalaryDto>().PreserveReferences().ReverseMap();
        //CreateMap<EmployeeSalaryDto, EmployeeSalary>().PreserveReferences().ReverseMap();

        CreateMap<EmployeeWorkTime, EmployeeWorkTimeDto>().PreserveReferences().ReverseMap();
        //CreateMap<EmployeeWorkTimeDto, EmployeeWorkTime>().PreserveReferences().ReverseMap();

        CreateMap<FirmClient, FirmClientDto>().PreserveReferences().ReverseMap();
        //CreateMap<FirmClientDto, FirmClient>().PreserveReferences().ReverseMap();

        //CreateMap<Role, RoleDto>().PreserveReferences().ReverseMap();
        CreateMap<RoleDto, Role>().PreserveReferences().ReverseMap();

        //CreateMap<Salary, SalaryDto>().PreserveReferences().ReverseMap();
        CreateMap<SalaryDto, Salary>().PreserveReferences().ReverseMap();

        CreateMap<WorkOrderActivity, WorkOrderActivityDto>().PreserveReferences().ReverseMap();
        //CreateMap<WorkOrderActivityDto, WorkOrderActivity>().PreserveReferences().ReverseMap();

        CreateMap<WorkOrderCost, WorkOrderCostDto>().PreserveReferences().ReverseMap();
        //CreateMap<WorkOrderCostDto, WorkOrderCost>().PreserveReferences().ReverseMap();

        CreateMap<WorkOrder, WorkOrderDto>().PreserveReferences().ReverseMap();
        //CreateMap<WorkOrderDto, WorkOrder>().PreserveReferences().ReverseMap();

        CreateMap<WorkTime, WorkTimeDto>().PreserveReferences().ReverseMap();
        //CreateMap<WorkTimeDto, WorkTime>().PreserveReferences().ReverseMap();

        CreateMap<EmployeeLogin, EmployeeLoginDto>().PreserveReferences().ReverseMap();
        //CreateMap<EmployeeLoginDto, EmployeeLogin>().PreserveReferences().ReverseMap();

        CreateMap<Employee, EmployeeLoginRecoveryDto>().PreserveReferences().ReverseMap();
    }
}
