using PlannerCRM.Server.Utilities;
using System.Security.AccessControl;

namespace PlannerCRM.Server.Profiles;

public class FirmClientProfile : Profile
{
    public FirmClientProfile()
    {
        CreateMap<FirmClient, FirmClientDto>()
            .MaxDepth(1)
            .ForMember(dest => dest.WorkOrders, cfg => cfg.Ignore())
            .AfterMap((s, d) => CustomTypeConverter<ICollection<WorkOrder>, List<WorkOrderDto>>.Convert(s.WorkOrders));
        CreateMap<FirmClientDto, FirmClient>().MaxDepth(1);
    }
}