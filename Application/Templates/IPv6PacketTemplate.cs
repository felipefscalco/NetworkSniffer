using PacketDotNet;
using System.Collections.ObjectModel;
using Packet = NetworkCommon.Models.Packet;

namespace Application.Templates
{
    public class IPv6PacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; set; }
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public string Protocol { get; set; }
        public string Version { get; set; }
        public string InternetHeaderLength { get; set; }
        public string Length { get; set; }
        public string TimeToLive { get; set; }
        public string FlowLabel { get; set; }
        public string TrafficClass { get; set; }
        public string PayloadLength { get; set; }

        public IPv6PacketTemplate(Packet packet)
        {
            SourceIp = packet.SourceIp;
            DestinationIp = packet.DestinationIP;
            Protocol = packet.Protocol.ToUpperInvariant();
            Version = packet.Version;
            Length = packet.Lenght;

            if (packet.IpPacket.Version == IPVersion.IPv6)
            {
                var ipv6 = packet.IpPacket.Extract<IPv6Packet>();
                InternetHeaderLength = ipv6.HeaderLength.ToString();
                TimeToLive = ipv6.TimeToLive.ToString();
                FlowLabel = ipv6.FlowLabel.ToString();
                TrafficClass = ipv6.TrafficClass.ToString();
                PayloadLength = ipv6.PayloadLength.ToString();
            }

            PopulatePacketContent(packet);
        }

        private void PopulatePacketContent(Packet packet)
        {
            var ipPacket = packet.IpPacket;
            switch (ipPacket.Protocol)
            {
                case ProtocolType.Tcp:
                    var tcpPacket = ipPacket.Extract<TcpPacket>();
                    if (tcpPacket != null)
                    {
                        //DestinationPort = tcpPacket.DestinationPort.ToString();
                        //SourcePort = tcpPacket.SourcePort.ToString();
                    }
                    break;

                case ProtocolType.Udp:
                    var udpPacket = ipPacket.Extract<UdpPacket>();
                    if (udpPacket != null)
                    {
                        //DestinationPort = udpPacket.DestinationPort.ToString();
                        //SourcePort = udpPacket.SourcePort.ToString();
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