using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class NTPSync
{
    public const string ntpServer1 = "ntp1.stratum2.ru";
    public const string ntpServer2 = "ntp.msk-ix.ru";

    public static TimeSpan GetTime(string ntpServer)
    {
        var ntpData = new byte[48];

        ntpData[0] = 0x1B;

        var adresses = Dns.GetHostEntry(ntpServer).AddressList;

        var ipEndPoint = new IPEndPoint(adresses[0], 123);

        using(var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);

            socket.ReceiveTimeout = 3000;

            socket.Send(ntpData);
            socket.Receive(ntpData);
        }

        const byte serverReplyTime = 40;

        ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
        ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        intPart = SwapEndiannes(intPart);
        fractPart = SwapEndiannes(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        var networkDataTime = (new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

        return networkDataTime.ToLocalTime().TimeOfDay;
    }

    private static uint SwapEndiannes(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }
}
