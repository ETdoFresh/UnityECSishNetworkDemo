using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECSish
{
    public class ECSEvent : MonoBehaviourSystem
    {
        private static Queue queue = Queue.Synchronized(new Queue());
        private static List<MonoBehaviourComponentData> eventComponents =
            new List<MonoBehaviourComponentData>();


        public static void Create<T>(Component component, params object[] objs) where T : MonoBehaviourComponentData =>
            Create<T>(component.gameObject, objs);

        public static void Create<T>(GameObject gameObject, params object[] objs) where T : MonoBehaviourComponentData
        {
            queue.Enqueue(new Func<MonoBehaviourComponentData>(() =>
            {
                var ev = gameObject.AddComponent<T>();
                for (int i = 0; i < objs.Length; i++)
                    typeof(T).GetFields()[i].SetValue(ev, objs[i]);
                return ev;
            }));
        }

        public static void ClearEvents()
        {
            foreach (var eventComponent in eventComponents)
                DestroyImmediate(eventComponent);

            eventComponents.Clear();
        }

        private void Update()
        {
            while (queue.Count > 0)
            {
                var func = (Func<MonoBehaviourComponentData>)queue.Dequeue();
                var eventComponent = func.Invoke();
                if (eventComponent)
                    eventComponents.Add(eventComponent);
            }
            Entity.UpdateDatabase();
        }
    }
}
