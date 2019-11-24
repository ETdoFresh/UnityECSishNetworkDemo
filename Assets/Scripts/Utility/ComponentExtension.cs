using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ComponentExtension
{
    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        return GetOrAddComponent<T>(component.gameObject);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var getComponent = gameObject.GetComponent<T>();
        if (getComponent) return getComponent;

        return gameObject.AddComponent<T>();
    }

    public static T CopyComponent<T>(this GameObject gameObject, Component component) where T : Component
    {
        if (!(component is T)) return default;
        Type type = component.GetType();

        T newComponent = gameObject.AddComponent(type) as T;

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(newComponent, pinfo.GetValue(component, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(newComponent, finfo.GetValue(component));
        }
        return newComponent;
    }

    public static Component CopyComponent(this GameObject gameObject, Component component)
        => CopyComponent<Component>(gameObject, component);

    private static bool Requires(Type obj, Type requirement)
    {
        //also check for m_Type1 and m_Type2 if required
        return Attribute.IsDefined(obj, typeof(RequireComponent)) &&
               Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
               .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
    }

    public static bool CanDestroy(this GameObject go, Type t)
    {
        return !go.GetComponents<Component>().Any(c => Requires(c.GetType(), t));
    }

    public static bool CanDestroy(this Component component)
    {
        return !component.GetComponents<Component>().Any(c => Requires(c.GetType(), component.GetType()));
    }
}