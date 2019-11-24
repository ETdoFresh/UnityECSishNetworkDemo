using ECSish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

public class SendFromWebSocketClientSystem : MonoBehaviourSystem
{
    public static Random Random { get; } = new Random();

    [DllImport("__Internal")] private static extern void SocketSend(int websocket, byte[] ptr, int length);

    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, WebSocketClient>())
        {
            var client = entity.Item2;
            if (!client) continue;

            var message = entity.Item1.message;
#if UNITY_WEBGL && !UNITY_EDITOR
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            SocketSend(client.websocketId, bytes, bytes.Length);
#else
            if (client.socket == null) continue;
            if (!client.socket.Connected) continue;
            var bytes = StringToBytes(message);
            client.stream.BeginWrite(bytes, 0, bytes.Length, OnSent, client);
#endif
        }
    }

    private void OnSent(IAsyncResult ar)
    {
            var client = (WebSocketClient)ar.AsyncState;
            client.stream.EndWrite(ar);
    }

    public static byte[] StringToBytes(string message, bool masked = true)
    {
        var mask = masked ? 0b_1000_0000 : 0;

        List<byte> bytes;

        // Bit description:
        // 0: Is final fragment of message
        // 1-3: Reserved bits. Must be 0.
        // 4-7: Opcode [0: continue, 1: text, 2: binary, 8: close, 9: ping, 10: pong]
        bytes = new List<byte> { 0b_1000_0001 };

        // 8: Maskbit
        // 9-15: Payload Length
        // 16-31: Extended Payload
        // 32-64: Extended Payload 2
        if (message.Length < 126)
        {
            bytes.Add((byte)(message.Length + mask));
        }
        else if (message.Length < 65536)
        {
            bytes.Add((byte)(126 + mask));
            bytes.AddRange(BitConverter.GetBytes((ushort)message.Length).Reverse());
        }
        else if ((ulong)message.Length <= 18446744073709551615)
        {
            bytes.Add((byte)(127 + mask));
            bytes.AddRange(BitConverter.GetBytes((ulong)message.Length).Reverse());
        }

        // +0 or +4: If masked, 4-byte mask
        var maskBytes = new List<byte>();
        if (masked)
        {
            for (int i = 0; i < 4; i++)
                maskBytes.Add((byte)Random.Next(-128, 128));
            bytes.AddRange(maskBytes);
        }


        // +message.Length: Data!
        var messageBytes = new List<byte>();
        messageBytes.AddRange(Encoding.UTF8.GetBytes(message));

        // Mask data if necessary
        if (masked)
            for (int i = 0; i < message.Length; i++)
            {
                var j = i % 4;
                messageBytes[i] = (byte)(messageBytes[i] ^ maskBytes[j]);
            }

        bytes.AddRange(messageBytes);
        return bytes.ToArray();
    }
}