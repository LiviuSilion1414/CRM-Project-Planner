using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlannerCRM.Shared;
public static class DtoShared
{
    public static string GetDescription(this Enum value)     
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
        return attributes?.Description;
    }

    public enum BaseRoles
    {
        [Description("66FBECFE-5CEF-4ACE-9A04-23AFBAF1A3C4")] USER = 0
    }
}
