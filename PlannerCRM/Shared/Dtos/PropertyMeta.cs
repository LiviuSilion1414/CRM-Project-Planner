using System.Reflection;

namespace PlannerCRM.Shared.Dtos;

public class PropertyMeta
{
    public PropertyInfo property { get; set; }
    public string label { get; set; }
    public Type underlyingType { get; set; }
    public bool isTextArea { get; set; }
    public int textAreaLength { get; set; }
    public bool isRequired { get; set; }
    public bool isRequiredIf { get; set; }
    public bool isNullable { get; set; }
    public bool isGuid { get; set; } = false;
    public Guid? id { get; set; }

}
