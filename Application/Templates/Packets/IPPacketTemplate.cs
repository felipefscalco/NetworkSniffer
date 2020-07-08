using Application.Templates.Abstractions;
using Application.Templates.Headers;
using NetworkCommon.Models;
using System.Collections.ObjectModel;

namespace Application.Templates.Packets
{
    public class IPPacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; }
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public string Protocol { get; set; }


        public IPPacketTemplate(Packet packet)
        {
            PacketContent = new ObservableCollection<IPacketTemplate>();

            SourceIp = packet.SourceIp;
            DestinationIp = packet.DestinationIP;
            Protocol = packet.Protocol.ToUpperInvariant();

            PopulatePacketContent(packet);
        }

        private void PopulatePacketContent(Packet packet)
        {
            var ipPacket = packet.IpPacket;
            switch (ipPacket.Version)
            {
                case PacketDotNet.IPVersion.IPv4:
                    var ipv4Header = new IPv4HeaderTemplate(packet);
                    PacketContent.Add(ipv4Header);
                    PacketContent.AddRange(ipv4Header.PacketContent);
                    break;

                case PacketDotNet.IPVersion.IPv6:
                    var ipv6Header = new IPv6HeaderTemplate(packet);
                    PacketContent.Add(ipv6Header);
                    PacketContent.AddRange(ipv6Header.PacketContent);
                    break;

                default:
                    break;
            }
        }
    }
}