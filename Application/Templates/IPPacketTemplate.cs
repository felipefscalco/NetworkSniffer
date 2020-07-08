using NetworkCommon.Models;
using System.Collections.ObjectModel;

namespace Application.Templates
{
    public class IPPacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; set; }
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
                    PacketContent.Add(new IPv4PacketTemplate(packet));
                    break;

                case PacketDotNet.IPVersion.IPv6:
                    PacketContent.Add(new IPv6PacketTemplate(packet));
                    break;

                default:
                    break;
            }
        }
    }
}