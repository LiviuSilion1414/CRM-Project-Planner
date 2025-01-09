namespace PlannerCRM.Client.Utilities;

public static class EntityHelper<TItem>
    where TItem : class, new()
{

    public static void InitializePropertiesRecursively(object obj)
    {
        if (obj == null)
            return;

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            if (!prop.CanWrite)
                continue;

            if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
                continue;

            var value = prop.GetValue(obj);
            if (value == null)
            {
                value = Activator.CreateInstance(prop.PropertyType);
                prop.SetValue(obj, value);
            }

            InitializePropertiesRecursively(value);
        }
    }
}
