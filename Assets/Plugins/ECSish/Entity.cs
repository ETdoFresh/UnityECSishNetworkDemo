﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECSish
{
    [Serializable]
    public class Entity
    {
        private static Entity instance;
        private static Entity Instance => instance == null ? instance = new Entity() : instance;

        public Dictionary<Type, HashSet<MonoBehaviourComponentData>> components = new Dictionary<Type, HashSet<MonoBehaviourComponentData>>();
        public Queue<MonoBehaviourComponentData> toBeAddedComponent = new Queue<MonoBehaviourComponentData>();
        public Queue<MonoBehaviourComponentData> toBeRemovedCompenent = new Queue<MonoBehaviourComponentData>();

        public Entity()
        {
            if (instance == null) instance = this;
        }

        public static IEnumerable<MonoBehaviourComponentData> GetComponents<T>() where T : MonoBehaviourComponentData
        {
            return GetComponents(typeof(T));
        }

        private static HashSet<MonoBehaviourComponentData> GetComponents(Type type)
        {
            if (!Instance.components.ContainsKey(type))
                Instance.components.Add(type, new HashSet<MonoBehaviourComponentData>());

            return Instance.components[type];
        }

        public static void Add(MonoBehaviourComponentData component)
        {
            Instance.toBeAddedComponent.Enqueue(component);
        }

        public static void Remove(MonoBehaviourComponentData component)
        {
            Instance.toBeRemovedCompenent.Enqueue(component);
        }

        public static void UpdateDatabase()
        {
            while (Instance.toBeAddedComponent.Count > 0)
            {
                var component = Instance.toBeAddedComponent.Dequeue();
                GetComponents(component.GetType()).Add(component);
            }

            while (Instance.toBeRemovedCompenent.Count > 0)
            {
                var component = Instance.toBeRemovedCompenent.Dequeue();
                GetComponents(component.GetType()).Remove(component);
            }
        }

        public static GameObject Instantiate(GameObject prefab)
        {
            return Object.Instantiate(prefab);
        }

        public static void Destroy(GameObject gameObject)
        {
            gameObject.AddComponent<EntityDestroyed>();
        }
    }
}