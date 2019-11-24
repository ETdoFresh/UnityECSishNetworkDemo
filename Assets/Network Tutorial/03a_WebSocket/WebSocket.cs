using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WebSocket
{
    public static Random Random { get; } = new Random();

    public static long PacketLength(IEnumerable<byte> bytes)
    {
        var length = PayloadLength(bytes);
        if (length == long.MaxValue) return length;

        var packetHeader = 2;

        var lengthOffset = 0;
        if (length < 126) lengthOffset = 0;
        else if (length < 65536) lengthOffset = 2;
        else lengthOffset = 8;

        var masked = (bytes.ToArray()[1] & 0b_1000_0000) > 0;
        var maskOffset = masked ? 4 : 0;

        return packetHeader + lengthOffset + maskOffset + length;
    }

    public static long PayloadLength(IEnumerable<byte> bytes)
    {
        if (bytes.Count() < 3)
            return long.MaxValue;

        long length = bytes.ElementAt(1) & 0b_0111_1111;
        var index = 2;

        if (length == 126)
        {
            var shrt = bytes.Skip(index).Take(2).Reverse().ToArray();
            return BitConverter.ToInt16(shrt, 0);
        }
        if (length == 127)
        {
            var lng = bytes.Skip(index).Take(8).Reverse().ToArray();
            return BitConverter.ToInt64(lng, 0);
        }
        else
            return length;
    }

    public static string BytesToString(byte[] bytes)
    {
        var isFinal = (bytes[0] & 0b_1000_0000) > 0;
        var opCode = bytes[0] & 0b_1000_1111;
        var closeConnectionRequested = opCode == 0x08;
        var masked = (bytes[1] & 0b_1000_0000) > 0;
        long length = bytes[1] & 0b_0111_1111;
        var index = 2;
        if (length == 126)
        {
            var shrt = bytes.Skip(index).Take(2).Reverse().ToArray();
            length = BitConverter.ToInt16(shrt, 0);
            index += 2;
        }
        if (length == 127)
        {
            var lng = bytes.Skip(index).Take(8).Reverse().ToArray();
            length = BitConverter.ToInt64(lng, 0);
            index += 8;
        }

        var maskBytes = new List<byte>();
        if (masked)
            for (int i = 0; i < 4; i++)
                maskBytes.Add(bytes[index + i]);

        index += masked ? 4 : 0;

        // +message.Length: Data!
        var messageBytes = new List<byte>();
        for (int i = 0; i < length; i++)
            messageBytes.Add(bytes[index + i]);

        // Mask data if necessary
        if (masked)
            for (int i = 0; i < length; i++)
            {
                var j = i % 4;
                messageBytes[i] = (byte)(messageBytes[i] ^ maskBytes[j]);
            }
        var message = Encoding.UTF8.GetString(messageBytes.ToArray());
        return message;
    }

    public static bool IsDiconnectPacket(IEnumerable<byte> bytes)
    {
        if (bytes.Count() == 0) return true;
        var opCode = bytes.ElementAt(0) & 0b_1000_1111;
        var closeConnectionRequested = opCode == 0x08;
        return closeConnectionRequested;
    }
}
