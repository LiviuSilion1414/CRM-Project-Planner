namespace PlannerCRM.Server.Utilities;

public static class CustomTypeConverter<TSource, TDestination>
{
    public static TDestination Convert(TSource source)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TSource, TDestination>().MaxDepth(1);
        });

        var mapper = config.CreateMapper();

        return mapper.Map<TSource, TDestination>(source);
    }
}
