﻿using ECSish;

public class OutputOnDisconnectedFromTCPServerEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedFromTCPServerEvent>())
        {
            var onDisconnectedEvent = entity.Item1;

            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;

            textMesh.text += $"{onDisconnectedEvent.client.host}:{onDisconnectedEvent.client.port} disconnected!\n";
        }
    }
}