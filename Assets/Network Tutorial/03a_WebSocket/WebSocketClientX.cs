//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Security;
//using System.Net.Sockets;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using UnityEngine;
//using Random = System.Random;

//[Serializable]
//public class WebSocketClientX : ClientOld
//{
//    private static Random Random { get; } = new Random();

//    public enum Security { Unsecured, Secured }
//    public Security security = Security.Unsecured;

//    public string url = "ws://localhost:9999";

//    public Socket clientSocket;
//    public Stream clientStream;

//    public Queue actionQueue = Queue.Synchronized(new Queue());
//    public readonly string eof = "<EOF>";
//    public readonly string eof = "<EOF>";
//    public readonly byte[] httpEof = Encoding.UTF8.GetBytes("\r\n\r\n");
//    public bool socketClosed = true;

//    public enum ReadyState { Connecting, Open, Closing, Closed };
//    public ReadyState readyState = ReadyState.Closed;

//#pragma warning disable IDE0044 // Add readonly modifier
//    private int websocket = -1;
//#pragma warning restore IDE0044 // Add readonly modifier

//    [DllImport("__Internal")] private static extern int SocketCreate(string url);
//    [DllImport("__Internal")] private static extern string SocketUrl(int websocket);
//    [DllImport("__Internal")] private static extern int SocketState(int websocket);
//    [DllImport("__Internal")] private static extern void SocketError(int websocket, byte[] ptr, int length);
//    [DllImport("__Internal")] private static extern void SocketSend(int websocket, byte[] ptr, int length);
//    [DllImport("__Internal")] private static extern int SocketRecvLength(int websocket);
//    [DllImport("__Internal")] private static extern void SocketRecv(int websocket, byte[] ptr, int length);
//    [DllImport("__Internal")] private static extern void SocketClose(int websocket);

//    private void OnEnable() => Connect();
//    private void OnDisable() => Disconnect();

//    private void Update()
//    {
//#if UNITY_WEBGL && !UNITY_EDITOR
//        if (websocket == -1)
//            return;

//        if (SocketState(websocket) == (int)ReadyState.Closed
//        || SocketState(websocket) == (int)ReadyState.Closing)
//        {
//            Disconnect();
//            return;
//        }

//        if (readyState == ReadyState.Closed || readyState == ReadyState.Connecting)
//            if (SocketState(websocket) == (int)ReadyState.Open)
//            {
//                clientEvents.onConnect.Invoke("Connected!");
//                readyState = ReadyState.Open;
//            }

//        if (SocketState(websocket) == (int)ReadyState.Open)
//        {
//            var received = Receive();
//            if (received != null)
//            {
//                var message = Encoding.UTF8.GetString(received, 0, received.Length);
//                var eofIndex = message.IndexOf(eof);
//                if (eofIndex >= 0)
//                    message = message.Substring(0, eofIndex);
//                messengerEvents.onReceive.Invoke(message);
//            }
//        }
//#endif
//    }

//    public byte[] Receive()
//    {
//        var length = SocketRecvLength(websocket);
//        if (length == 0) return null;
//        var bytes = new byte[length];
//        SocketRecv(websocket, bytes, bytes.Length);
//        return bytes;
//    }

//    public override void Connect()
//    {
////        url = GUIValues.GetUserSpecifiedOrExisting("url", url);
////        try
////        {
////#if UNITY_WEBGL && !UNITY_EDITOR
////            websocket = SocketCreate(url);
////            readyState = ReadyState.Connecting;
////#else
////            var uri = new Uri(url);
////            var host = uri.Host;
////            var path = uri.PathAndQuery;
////            var port = uri.Port;
////            IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
////            IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(i
////                => i.AddressFamily == AddressFamily.InterNetwork);
////            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

////            clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
////            clientSocket.BeginConnect(remoteEP, ConnectCallback, clientSocket);
////#endif
////        }
////        catch (Exception e)
////        {
////            Debug.LogWarning(e.Message);
////            //MainThread.Run(onError, e.ToString());
////        }
//    }

//    private void ConnectCallback(IAsyncResult ar)
//    {
//        //try
//        //{
//        //    var uri = new Uri(url);
//        //    var host = uri.Host;
//        //    var path = uri.PathAndQuery;

//        //    Socket clientSocket = (Socket)ar.AsyncState;
//        //    clientSocket.EndConnect(ar);
//        //    socketClosed = false;
//        //    Stream stream = new NetworkStream(clientSocket);
//        //    if (security == Security.Secured)
//        //    {
//        //        stream = new SslStream(stream, false,
//        //        new RemoteCertificateValidationCallback(ValidateServerCertificate),
//        //        null);
//        //        ((SslStream)stream).AuthenticateAsClient(host);
//        //    }
//        //    clientStream = stream;
//        //    var buffer = new byte[2048];
//        //    var state = new State(clientSocket, buffer, new List<byte>(), stream, new StringBuilder());

//        //    var eol = "\r\n";
//        //    var handshake = "GET " + path + " HTTP/1.1" + eol;
//        //    handshake += "Host: " + host + eol;
//        //    handshake += "Upgrade: websocket" + eol;
//        //    handshake += "Connection: Upgrade" + eol;
//        //    handshake += "Sec-WebSocket-Key: V2ViU29ja2V0Q2xpZW50" + eol;
//        //    handshake += "Sec-WebSocket-Version: 13" + eol;
//        //    handshake += eol;

//        //    var handshakeBytes = Encoding.UTF8.GetBytes(handshake);
//        //    stream.Write(handshakeBytes, 0, handshakeBytes.Length);
//        //    stream.BeginRead(buffer, 0, buffer.Length, ReceiveHandshakeCallback, state);
//        //}
//        //catch (Exception e)
//        //{
//        //    Debug.LogWarning(e.Message);
//        //    //MainThread.Run(onError, e.ToString());
//        //}
//    }

//    private static bool ValidateServerCertificate(object sender,
//        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
//    {
//        //if (sslPolicyErrors == SslPolicyErrors.None)
//        //    return true;

//        //Debug.LogErrorFormat("Certificate error: {0}", sslPolicyErrors);

//        //// Do not allow this client to communicate with unauthenticated servers.
//        return false;
//    }

//    private void ReceiveHandshakeCallback(IAsyncResult ar)
//    {
//        //try
//        //{
//        //    var state = (State)ar.AsyncState;
//        //    var clientSocket = state.Item1;
//        //    var buffer = state.Item2;
//        //    var received = state.Item3;
//        //    var stream = state.Item4;

//        //    int bytesRead = stream.EndRead(ar);
//        //    received.AddRange(buffer.Take(bytesRead));

//        //    if (bytesRead > 0)
//        //    {
//        //        var eofIndex = received.IndexOf(httpEof);
//        //        if (eofIndex == -1)
//        //        {
//        //            stream.BeginRead(buffer, 0, buffer.Length, ReceiveHandshakeCallback, state);
//        //            return;
//        //        }

//        //        var message = Encoding.UTF8.GetString(received.ToArray(), 0, eofIndex);
//        //        //MainThread.Run(messengerEvents.onReceive, message);
//        //        received.RemoveRange(0, eofIndex + httpEof.Length);
//        //        MainThread.Run(clientEvents.onConnect, string.Format("Socket connected to {0}", clientSocket.RemoteEndPoint.ToString()));
//        //        isConnected = true;
//        //        stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, state);
//        //    }
//        //    else
//        //    {
//        //        Disconnect();
//        //    }
//        //}
//        //catch (Exception e)
//        //{
//        //    Debug.LogWarning(e.Message);
//        //    //Main.MainThread.Run(onError, e.ToString());
//        //}
//    }

//    private void ReceiveCallback(IAsyncResult ar)
//        => MainThread.Run(ReceiveCallbackOnMainThread, ar);

//    private void ReceiveCallbackOnMainThread(IAsyncResult ar)
//    {
//        //try
//        //{
//        //    var state = (State)ar.AsyncState;
//        //    var buffer = state.Item2;
//        //    var received = state.Item3;
//        //    var stream = state.Item4;
//        //    var sb = state.Item5;

//        //    if (!clientSocket.Connected)
//        //        return;

//        //    int bytesRead = stream.EndRead(ar);
//        //    received.AddRange(buffer.Take(bytesRead));

//        //    if (bytesRead > 0)
//        //    {
//        //        while (received.Count >= PacketLength(received))
//        //        {
//        //            var message = BytesToString(received.ToArray());
//        //            sb.Append(message);
//        //            received.RemoveRange(0, (int)PacketLength(received));
//        //            var eofIndex = sb.ToString().IndexOf(eof);
//        //            if (eofIndex == -1)
//        //                continue;

//        //            message = sb.ToString().Substring(0, eofIndex);
//        //            sb.Remove(0, eofIndex + eof.Length);
//        //            MainThread.Run(messengerEvents.onReceive, message);
//        //        }
//        //        stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, state);
//        //    }
//        //    else
//        //    {
//        //        Disconnect();
//        //    }
//        //}
//        //catch (Exception e)
//        //{
//        //    Debug.LogWarning(e.Message);
//        //    //MainThread.Run(onError, e.ToString());
//        //}
//    }

//    public override void Send(string data)
//    {
//#if UNITY_WEBGL && !UNITY_EDITOR
//        byte[] bytes = Encoding.UTF8.GetBytes(data);
//        SocketSend(websocket, bytes, bytes.Length);
//#else
//        try
//        {
//            if (clientSocket == null) return;
//            var bytes = StringToBytes(data);
//            clientStream.BeginWrite(bytes, 0, bytes.Length, SendCallback, clientStream);
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning(e.Message);
//        }
//#endif
//        MainThread.Run(messengerEvents.onSend, data);
//    }

//    private void SendCallback(IAsyncResult ar)
//    {
//        var clientStream = (Stream)ar.AsyncState;
//        try
//        {
//            clientStream.EndWrite(ar);
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning(e.Message);
//            //MainThread.Run(onError, e.ToString());
//        }
//    }

//    public override void Disconnect()
//    {
//        isConnected = false;
//#if UNITY_WEBGL && !UNITY_EDITOR
//        SocketClose(websocket);
//#else
//        try
//        {
//            if (clientSocket.RemoteEndPoint == null) return;
//            if (clientSocket.Connected)
//                clientSocket.Shutdown(SocketShutdown.Both);
//            clientSocket.Close();
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning(e.Message);
//        }

//        try
//        {
//            clientStream.Close();
//        }
//        catch (Exception e)
//        {
//            Debug.LogWarning(e.Message);
//        }
//#endif
//        MainThread.Run(clientEvents.onDisconnect, "Socket disconnected");
//        socketClosed = true;
//    }

//    public class State : Tuple<Socket, byte[], List<byte>, Stream, StringBuilder>
//    {
//        public State(Socket item1, byte[] item2, List<byte> item3, Stream item4, StringBuilder item5)
//            : base(item1, item2, item3, item4, item5) { }
//    }

//    public static byte[] StringToBytes(string message, bool masked = true)
//    {
//        var mask = masked ? 0b_1000_0000 : 0;

//        List<byte> bytes;

//        // Bit description:
//        // 0: Is final fragment of message
//        // 1-3: Reserved bits. Must be 0.
//        // 4-7: Opcode [0: continue, 1: text, 2: binary, 8: close, 9: ping, 10: pong]
//        bytes = new List<byte> { 0b_1000_0001 };

//        // 8: Maskbit
//        // 9-15: Payload Length
//        // 16-31: Extended Payload
//        // 32-64: Extended Payload 2
//        if (message.Length < 126)
//        {
//            bytes.Add((byte)(message.Length + mask));
//        }
//        else if (message.Length < 65536)
//        {
//            bytes.Add((byte)(126 + mask));
//            bytes.AddRange(BitConverter.GetBytes((ushort)message.Length).Reverse());
//        }
//        else if ((ulong)message.Length <= 18446744073709551615)
//        {
//            bytes.Add((byte)(127 + mask));
//            bytes.AddRange(BitConverter.GetBytes((ulong)message.Length).Reverse());
//        }

//        // +0 or +4: If masked, 4-byte mask
//        var maskBytes = new List<byte>();
//        if (masked)
//        {
//            for (int i = 0; i < 4; i++)
//                maskBytes.Add((byte)Random.Next(-128, 128));
//            bytes.AddRange(maskBytes);
//        }


//        // +message.Length: Data!
//        var messageBytes = new List<byte>();
//        messageBytes.AddRange(Encoding.UTF8.GetBytes(message));

//        // Mask data if necessary
//        if (masked)
//            for (int i = 0; i < message.Length; i++)
//            {
//                var j = i % 4;
//                messageBytes[i] = (byte)(messageBytes[i] ^ maskBytes[j]);
//            }

//        bytes.AddRange(messageBytes);
//        return bytes.ToArray();
//    }

//    public static long PacketLength(IEnumerable<byte> bytes)
//    {
//        var length = PayloadLength(bytes);
//        if (length == long.MaxValue) return length;

//        var packetHeader = 2;

//        var lengthOffset = 0;
//        if (length < 126) lengthOffset = 0;
//        else if (length < 65536) lengthOffset = 2;
//        else lengthOffset = 8;

//        var masked = (bytes.ToArray()[1] & 0b_1000_0000) > 0;
//        var maskOffset = masked ? 4 : 0;

//        return packetHeader + lengthOffset + maskOffset + length;
//    }

//    public static long PayloadLength(IEnumerable<byte> bytes)
//    {
//        if (bytes.Count() < 3)
//            return long.MaxValue;

//        long length = bytes.ElementAt(1) & 0b_0111_1111;
//        var index = 2;

//        if (length == 126)
//        {
//            var shrt = bytes.Skip(index).Take(2).Reverse().ToArray();
//            return BitConverter.ToInt16(shrt, 0);
//        }
//        if (length == 127)
//        {
//            var lng = bytes.Skip(index).Take(8).Reverse().ToArray();
//            return BitConverter.ToInt64(lng, 0);
//        }
//        else
//            return length;
//    }

//    public static string BytesToString(byte[] bytes)
//    {
//        var isFinal = (bytes[0] & 0b_1000_0000) > 0;
//        var opCode = bytes[0] & 0b_1000_1111;
//        var closeConnectionRequested = opCode == 0x08;
//        var masked = (bytes[1] & 0b_1000_0000) > 0;
//        long length = bytes[1] & 0b_0111_1111;
//        var index = 2;
//        if (length == 126)
//        {
//            var shrt = bytes.Skip(index).Take(2).Reverse().ToArray();
//            length = BitConverter.ToInt16(shrt, 0);
//            index += 2;
//        }
//        if (length == 127)
//        {
//            var lng = bytes.Skip(index).Take(8).Reverse().ToArray();
//            length = BitConverter.ToInt64(lng, 0);
//            index += 8;
//        }

//        var maskBytes = new List<byte>();
//        if (masked)
//            for (int i = 0; i < 4; i++)
//                maskBytes.Add(bytes[index + i]);

//        index += masked ? 4 : 0;

//        // +message.Length: Data!
//        var messageBytes = new List<byte>();
//        for (int i = 0; i < length; i++)
//            messageBytes.Add(bytes[index + i]);

//        // Mask data if necessary
//        if (masked)
//            for (int i = 0; i < length; i++)
//            {
//                var j = i % 4;
//                messageBytes[i] = (byte)(messageBytes[i] ^ maskBytes[j]);
//            }
//        var message = Encoding.UTF8.GetString(messageBytes.ToArray());
//        return message;
//    }
//}
