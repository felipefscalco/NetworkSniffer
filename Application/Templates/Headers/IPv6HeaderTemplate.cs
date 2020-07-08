using Application.Templates.Abstractions;
using Application.Templates.Packets;
using PacketDotNet;
using System.Collections.ObjectModel;
using Packet = NetworkCommon.Models.Packet;

namespace Application.Templates.Headers
{
    public class IPv6HeaderTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; }
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

        public IPv6HeaderTemplate(Packet packet)
        {
            PacketContent = new ObservableCollection<IPacketTemplate>();

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
                        PacketContent.Add(new TcpPacketTemplate(tcpPacket));
                    break;

                case ProtocolType.Udp:
                    var udpPacket = ipPacket.Extract<UdpPacket>();
                    if (udpPacket != null)
                        PacketContent.Add(new UdpPacketTemplate(udpPacket));
                    break;

                case ProtocolType.IcmpV6:
                    var icmpv6Packet = ipPacket.Extract<IcmpV6Packet>();
                    if (icmpv6Packet != null)
                        PacketContent.Add(new IcmpV6PacketTemplate(icmpv6Packet));
                    break;

                case ProtocolType.Igmp:
                    var igmpPacket = ipPacket.Extract<IgmpV2Packet>();
                    if (igmpPacket != null)
                        PacketContent.Add(new IgmpPacketTemplate(igmpPacket));
                    break;

                default:
                    break;
            }
        }
    }
}