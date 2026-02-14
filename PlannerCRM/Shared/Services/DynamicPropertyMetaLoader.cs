using PlannerCRM.Shared.Dtos;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Reflection;

namespace PlannerCRM.Shared.Services;

public static class DynamicPropertyMetaLoader
{
    public static bool ShowOnlyRequired = true;
    static readonly NullabilityInfoContext nullabilityContext = new();
    static readonly List<Type> SupportedTypes = new()
    {
        typeof(string),
        typeof(int),
        typeof(int?),
        typeof(decimal),
        typeof(decimal?),
        typeof(double),
        typeof(double?),
        typeof(bool),
        typeof(bool?),
        typeof(DateTime),
        typeof(DateTime?)
    };

    public static bool IsSupported(Type type) => SupportedTypes.Contains(type);

    public static List<PropertyMeta> GetModelProperties(object model)
    {
        return model.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite &&
                                p.SetMethod != null &&
                                (SupportedTypes.Contains(p.PropertyType)) &&
                                ((ShowOnlyRequired == false) ||
                                (ShowOnlyRequired == true &&
                                    p.GetCustomAttribute<RequiredAttribute>() != null &&
                                    p.GetCustomAttribute<DescriptionAttribute>() != null)))
                    .Select(p =>
                    {
                        var underlyingType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                        var nullability = nullabilityContext.Create(p);

                        bool isNullable =
                            Nullable.GetUnderlyingType(p.PropertyType) != null ||
                            nullability.ReadState == NullabilityState.Nullable;

                        return new PropertyMeta
                        {
                            property = p,
                            isNullable = isNullable,
                            underlyingType = underlyingType,
                            label = p.GetCustomAttribute<DescriptionAttribute>()?.Description ?? p.Name,
                            isRequired = p.GetCustomAttribute<RequiredAttribute>() != null,
                            isTextArea = p.GetCustomAttribute<MaxLengthAttribute>() != null,
                            textAreaLength = p.GetCustomAttribute<MaxLengthAttribute>() != null ? p.GetCustomAttribute<MaxLengthAttribute>().Length : 0
                        };
                    })
                    .ToList();
    }
}
