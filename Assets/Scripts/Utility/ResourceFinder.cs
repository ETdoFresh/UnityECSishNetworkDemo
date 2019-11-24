using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceFinder : ScriptableObject
{
    private static ResourceFinder instance = null;
    public List<ObjectId> resources;

    public static ResourceFinder Instance => GetOrFindInstance();
    public static T Get<T>(int instanceId) where T : Object => Instance.IGet<T>(instanceId);
    public static int GetId(Object value) => Instance.IGetId(value);

    private static ResourceFinder GetOrFindInstance()
    {
        if (!instance)
            instance = Resources.Load<ResourceFinder>("ResourceFinder");
        return instance;
    }

    public static T Enum<T>(string value)
    {
        return (T)System.Enum.Parse(typeof(T), value);
    }

    private int IGetId(Object value)
    {
        foreach (var resource in resources)
            if (resource.obj == value)
                return resource.id;

        var newId = NextAvailableId();
        resources.Add(new ObjectId { id = newId, obj = value });

        return newId;
    }

    private T IGet<T>(int id) where T : Object
    {
        foreach (var resource in resources)
            if (resource.id == id)
                return (T)resource.obj;
        return null;
    }

    private int NextAvailableId()
    {
        for (int i = 0; i <= int.MaxValue; i++)
            if (!resources.Any(r => r.id == i))
                return i;
        throw new System.Exception("Ran out of IDs on ResourceFinder");
    }

    [System.Serializable]
    public class ObjectId
    {
        public int id;
        public Object obj;
    }
}