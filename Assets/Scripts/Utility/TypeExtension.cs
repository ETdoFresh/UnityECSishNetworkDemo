using System;

public static class TypeExtension
{
    public static T CreateInstance<T>(this Type type, params object[] args)
    {
        var obj = Activator.CreateInstance(type, args);
        return (T)obj;
    }

    public static object GetDefaultValue(this Type t)
    {
        if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
            return Activator.CreateInstance(t);
        else
            return null;
    }
}