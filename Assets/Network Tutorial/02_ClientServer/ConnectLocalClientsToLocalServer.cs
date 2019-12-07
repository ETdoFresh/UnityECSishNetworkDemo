﻿using ECSish;
using System.Linq;

public class ConnectLocalClientsToLocalServer : MonoBehaviourSystem
{
    private void Update()
    {
        var servers = GetEntities<LocalServer>().Select(e => e.Item1);
        var clients = GetAllEntities<LocalClient>().Select(e => e.Item1);

        foreach (var server in servers)
        {
            var newClients = clients.Except(server.clients);
            foreach (var newClient in newClients)
            {
                server.clients.Add(newClient);
                newClient.server = server;
                EventSystem.Add(() => server.gameObject.AddComponent<OnLocalClientAccepted>());
                EventSystem.Add(() => newClient.gameObject.AddComponent<OnLocalClientAccepted>());
            }

            var disconnectedClients = server.clients.Except(clients);
            for (int i = server.clients.Count - 1; i >= 0; i--)
            {
                var disconnectedClient = server.clients[i];
                if (disconnectedClients.Contains(disconnectedClient))
                {
                    server.clients.RemoveAt(i);
                    EventSystem.Add(() => server.gameObject.AddComponent<OnLocalClientDisconnected>());
                    EventSystem.Add(() => disconnectedClient.gameObject.AddComponent<OnLocalClientDisconnected>());
                }
            }
        }
    }
}