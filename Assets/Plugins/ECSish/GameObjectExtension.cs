using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECSish
{
    public static class ComponentExtension
    {
        public static T GetComponent<T>(this Component component, bool includeInactive)
            where T : MonoBehaviour
        {
            return GetComponents<T>(component, includeInactive).FirstOrDefault();
        }

        public static IEnumerable<T> GetComponents<T>(this Component component, bool includeInactive)
            where T : MonoBehaviour
        {
            foreach (var c in component.gameObject.GetComponents<T>())
                if (c.enabled)
                    yield return c;
                else if (includeInactive && !c.enabled)
                    yield return c;
        }
    }
}
