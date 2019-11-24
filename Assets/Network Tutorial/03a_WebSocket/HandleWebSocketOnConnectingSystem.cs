using ECSish;
using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

public class HandleWebSocketOnConnectingSystem : MonoBehaviourSystem
{
    [DllImport("__Internal")] private static extern int SocketCreate(string url);

    private void Update()
    {
        foreach (var entity in GetEntities<OnConnectingEvent, WebSocketClient>())
        {
            var client = entity.Item2;
            if (client.socket != null) continue;
            if (client.isConnected) continue;

#if UNITY_WEBGL && !UNITY_EDITOR
                client.websocketId = SocketCreate(client.url);
                client.readyState = WebSocketClient.ReadyState.Connecting;
#else
            var uri = new Uri(client.url);
            var host = uri.Host;
            var path = uri.PathAndQuery;
            var port = uri.Port;
            var ipHostInfo = Dns.GetHostEntry(host);
            var ipAddress = ipHostInfo.AddressList.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);
            var remoteEP = new IPEndPoint(ipAddress, port);

            client.socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.socket.BeginConnect(remoteEP, OnConnected, client);
#endif
        }
    }

    private void OnConnected(IAsyncResult ar)
    {
        var client = (WebSocketClient)ar.AsyncState;
        if (client.socket.Connected)
        {
            client.isConnected = true;
            EventSystem.Add(() => client.gameObject.AddComponent<OnConnectedEvent>());
            StartHandShake(client);
        }
        else
        {
            client.socket.Dispose();
            client.socket = null;
            EventSystem.Add(() => client.gameObject.AddComponent<OnCouldNotConnectEvent>());
        }
    }

    private void StartHandShake(WebSocketClient client)
    {
        var uri = new Uri(client.url);
        var host = uri.Host;
        var path = uri.PathAndQuery;

        client.stream = new NetworkStream(client.socket);
        if (uri.Scheme.ToLower() == "wss")
        {
            client.stream = new SslStream(client.stream, false,
            new RemoteCertificateValidationCallback(ValidateServerCertificate),
            null);
            ((SslStream)client.stream).AuthenticateAsClient(host);
        }

        var eol = "\r\n";
        var handshake = "GET " + path + " HTTP/1.1" + eol;
        handshake += "Host: " + host + eol;
        handshake += "Upgrade: websocket" + eol;
        handshake += "Connection: Upgrade" + eol;
        handshake += "Sec-WebSocket-Key: V2ViU29ja2V0Q2xpZW50" + eol;
        handshake += "Sec-WebSocket-Version: 13" + eol;
        handshake += eol;
        var handshakeBytes = Encoding.UTF8.GetBytes(handshake);
        client.stream.Write(handshakeBytes, 0, handshakeBytes.Length);
        client.stream.BeginRead(client.buffer, 0, client.buffer.Length, ReceiveHandshake, client);
    }

    private void ReceiveHandshake(IAsyncResult ar)
    {
        var client = (WebSocketClient)ar.AsyncState;
        try
        {
            int bytesRead = client.stream.EndRead(ar);
            client.received.AddRange(client.buffer.Take(bytesRead));

            if (bytesRead > 0)
            {
                var httpEof = Encoding.UTF8.GetBytes("\r\n\r\n");
                var eofIndex = client.received.IndexOf(httpEof);
                if (eofIndex == -1)
                {
                    client.stream.BeginRead(client.buffer, 0, client.buffer.Length, ReceiveHandshake, client);
                    return;
                }

                var message = Encoding.UTF8.GetString(client.received.ToArray(), 0, eofIndex);
                client.received.RemoveRange(0, eofIndex + httpEof.Length);
                client.hasPerformedHandshake = true;
                EventSystem.Add(() =>
                {
                    var onReceiveEvent = client.gameObject.AddComponent<OnReceiveEvent>();
                    onReceiveEvent.message = message;
                    return onReceiveEvent;
                });
            }
            else
                throw new Exception("Handshake failed: Disconnecting...");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            if (client.socket != null)
            {
                if (client.socket.Connected) client.socket.Disconnect(false);
                client.socket.Dispose();
                client.socket = null;
                EventSystem.Add(() => client.gameObject.AddComponent<OnDisconnectedEvent>());
            }
            client.isConnected = false;
            client.hasPerformedHandshake = false;
            client.isReceiving = false;
        }
    }

    private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        Debug.LogErrorFormat("Certificate error: {0}", sslPolicyErrors);

        // Do not allow this client to communicate with unauthenticated servers.
        return false;
    }
}
