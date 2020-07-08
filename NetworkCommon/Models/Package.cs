using PacketDotNet;
using System;

namespace NetworkCommon.Models
{
    public class Packet
    {
        public IPPacket IpPacket { get; }

        public int PacketNumber { get; }

        public string SourceIp { get; }

        public string ReceivedTime { get; }

        public string SourcePort { get; private set; }

        public string DestinationIP { get; }

        public string DestinationPort { get; private set; }

        public string Protocol { get; }

        public string Lenght { get; }

        public string Version { get; }

        public Packet(int packetNumber, TimeSpan time, IPPacket packet)
        {
            IpPacket = packet;

            PacketNumber = packetNumber;
            TimeSpan.TryParseExact(time.ToString(), @"s\.fff", null, out var currentTime);
            ReceivedTime = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);//ToLocalTime().ToString("HH:mm tt");

            SourceIp = packet.SourceAddress.ToString();
            DestinationIP = packet.DestinationAddress.ToString();

            Protocol = packet.Protocol.ToString().ToUpperInvariant();
            Lenght = packet.TotalPacketLength.ToString();
            Version = packet.Version.ToString();

            ExtractPacket(packet);
        }

        private void ExtractPacket(IPPacket packet)
        {
            switch (packet.Protocol)
            {
                case ProtocolType.Tcp:
                    var tcpPacket = packet.Extract<TcpPacket>();
                    if (tcpPacket != null)
                    {
                        DestinationPort = tcpPacket.DestinationPort.ToString();
                        SourcePort = tcpPacket.SourcePort.ToString();
                    }
                    break;

                case ProtocolType.Udp:
                    var udpPacket = packet.Extract<UdpPacket>();
                    if (udpPacket != null)
                    {
                        DestinationPort = udpPacket.DestinationPort.ToString();
                        SourcePort = udpPacket.SourcePort.ToString();
                    }
                    break;

                case ProtocolType.IcmpV6:
                    break;

                default:
                    break;
            }
        }
    }
}