using System.Reflection;

namespace PlannerCRM.Shared.Dtos;

public class PropertyMeta
{
    public PropertyInfo property { get; set; }
    public string label { get; set; }
    public Type underlyingType { get; set; }
    public bool isRequired { get; set; }
    public bool isNullable { get; set; }
}
