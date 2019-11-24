using ECSish;

public class StartClientConnectOnEnable : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnEnableEvent, Client>())
        {
            var client = entity.Item2;
            EventSystem.Add(() => client.gameObject.AddComponent<OnConnectingEvent>());
        }
    }
}
