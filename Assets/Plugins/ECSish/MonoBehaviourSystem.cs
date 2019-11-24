using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECSish
{
    public class MonoBehaviourSystem : MonoBehaviour
    {
        public IEnumerable<Tuple<T1>> GetEntities<T1>()
            where T1 : MonoBehaviourComponentData
        {
            foreach (var component1 in Entity.GetComponents<T1>())
            {
                if (!component1) continue;
                if (!component1.enabled) continue;
                if (component1.gameObject.scene.handle != gameObject.scene.handle && gameObject.scene.handle != DontDestroyOnLoadScene.Scene.handle) continue;
                yield return new Tuple<T1>((T1)component1);
            }
        }

        public IEnumerable<Tuple<T1, T2>> GetEntities<T1, T2>()
            where T1 : MonoBehaviourComponentData
            where T2 : MonoBehaviourComponentData
        {
            foreach (var component1 in Entity.GetComponents<T1>())
            {
                if (!component1) continue;
                if (!component1.enabled) continue;
                if (component1.gameObject.scene.handle != gameObject.scene.handle && gameObject.scene.handle != DontDestroyOnLoadScene.Scene.handle) continue;
                foreach (var component2 in Entity.GetComponents<T2>())
                {
                    if (!component2) continue;
                    if (!component2.enabled) continue;
                    if (component2.gameObject != component1.gameObject) continue;
                    yield return new Tuple<T1, T2>((T1)component1, (T2)component2);
                }
            }
        }

        public IEnumerable<Tuple<T1, T2, T3>> GetEntities<T1, T2, T3>()
            where T1 : MonoBehaviourComponentData
            where T2 : MonoBehaviourComponentData
            where T3 : MonoBehaviourComponentData
        {
            foreach (var component1 in Entity.GetComponents<T1>())
            {
                if (!component1) continue;
                if (!component1.enabled) continue;
                if (component1.gameObject.scene.handle != gameObject.scene.handle && gameObject.scene.handle != DontDestroyOnLoadScene.Scene.handle) continue;
                foreach (var component2 in Entity.GetComponents<T2>())
                {
                    if (!component2) continue;
                    if (!component2.enabled) continue;
                    if (component2.gameObject != component1.gameObject) continue;
                    foreach (var component3 in Entity.GetComponents<T3>())
                    {
                        if (!component3) continue;
                        if (!component3.enabled) continue;
                        if (component3.gameObject != component2.gameObject) continue;
                        yield return new Tuple<T1, T2, T3>((T1)component1, (T2)component2, (T3)component3);
                    }
                }
            }
        }

        public IEnumerable<Tuple<T1, T2, T3, T4>> GetEntities<T1, T2, T3, T4>()
            where T1 : MonoBehaviourComponentData
            where T2 : MonoBehaviourComponentData
            where T3 : MonoBehaviourComponentData
            where T4 : MonoBehaviourComponentData
        {
            foreach (var component1 in Entity.GetComponents<T1>())
            {
                if (!component1) continue;
                if (!component1.enabled) continue;
                if (component1.gameObject.scene.handle != gameObject.scene.handle && gameObject.scene.handle != DontDestroyOnLoadScene.Scene.handle) continue;
                foreach (var component2 in Entity.GetComponents<T2>())
                {
                    if (!component2) continue;
                    if (!component2.enabled) continue;
                    if (component2.gameObject != component1.gameObject) continue;
                    foreach (var component3 in Entity.GetComponents<T3>())
                    {
                        if (!component3) continue;
                        if (!component3.enabled) continue;
                        if (component3.gameObject != component2.gameObject) continue;
                        foreach (var component4 in Entity.GetComponents<T4>())
                        {
                            if (!component4) continue;
                            if (!component4.enabled) continue;
                            if (component4.gameObject != component3.gameObject) continue;
                            yield return new Tuple<T1, T2, T3, T4>((T1)component1, (T2)component2, (T3)component3, (T4)component4);
                        }
                    }
                }
            }
        }

        public Tuple<T1> GetEntity<T1>() where T1 : MonoBehaviourComponentData
            => GetEntities<T1>().FirstOrDefault();

        public Tuple<T1, T2> GetEntity<T1, T2>()
            where T1 : MonoBehaviourComponentData
            where T2 : MonoBehaviourComponentData
            => GetEntities<T1, T2>().FirstOrDefault();

        public Tuple<T1, T2, T3> GetEntity<T1, T2, T3>()
            where T1 : MonoBehaviourComponentData
            where T2 : MonoBehaviourComponentData
            where T3 : MonoBehaviourComponentData
            => GetEntities<T1, T2, T3>().FirstOrDefault();

        public Tuple<T1, T2, T3, T4> GetEntity<T1, T2, T3, T4>()
            where T1 : MonoBehaviourComponentData
            where T2 : MonoBehaviourComponentData
            where T3 : MonoBehaviourComponentData
            where T4 : MonoBehaviourComponentData
            => GetEntities<T1, T2, T3, T4>().FirstOrDefault();
    }
}