using UnityEngine;

namespace ECSish
{
    public static class EventUtility
    {
        public static void CreateOnSendEvent(GameObject gameObject, string message)
        {
            EventSystem.Add(() =>
            {
                if (!gameObject) return null;
                var onSendEvent = gameObject.AddComponent<OnSendEvent>();
                onSendEvent.message = message;
                onSendEvent.args = message.Split(' ');
                return onSendEvent;
            });
        }

        public static void CreateOnSendFromSessionEvent(GameObject gameObject, string message)
        {
            EventSystem.Add(() =>
            {
                if (!gameObject) return null;
                var onSendEvent = gameObject.AddComponent<OnSendFromSessionEvent>();
                onSendEvent.message = message;
                onSendEvent.args = message.Split(' ');
                return onSendEvent;
            });
        }
    }
}