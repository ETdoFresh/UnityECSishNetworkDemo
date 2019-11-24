using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECSish
{
    public class EventSystem : MonoBehaviourSystem
    {
        private static Queue queue = Queue.Synchronized(new Queue());
        private static List<MonoBehaviourComponentData> eventComponents =
            new List<MonoBehaviourComponentData>();

        public static void Add(Func<MonoBehaviourComponentData> func)
        {
            queue.Enqueue(func);
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
