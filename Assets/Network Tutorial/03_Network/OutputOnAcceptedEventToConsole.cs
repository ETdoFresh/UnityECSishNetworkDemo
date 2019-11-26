using ECSish;

public class OutputOnAcceptedEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnAcceptedEvent>())
        {
            var onAcceptedEvent = entity.Item1;
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;
            object connection = onAcceptedEvent.connection;
            if (connection is TCPClientConnection tcpConnection)
                textMesh.text += $"{tcpConnection.host}:{tcpConnection.port} connected!\n";
            else
                textMesh.text += $"Client connected!\n";
        }
    }
}
