using PacketDotNet;
using System.Collections.ObjectModel;
using Packet = NetworkCommon.Models.Packet;

namespace Application.Templates
{
    public class IPv4PacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; set; }
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public string Protocol { get; set; }
        public string Version { get; set; }
        public string InternetHeaderLength { get; set; }
        public string TypeOfService { get; set; }
        public string Length { get; set; }
        public string FragmentOffset { get; set; }
        public string TimeToLive { get; set; }
        public string HeaderChecksum { get; set; }
        public string PayloadLength { get; set; }

        public IPv4PacketTemplate(Packet packet)
        {
            SourceIp = packet.SourceIp;
            DestinationIp = packet.DestinationIP;
            Protocol = packet.Protocol.ToUpperInvariant();
            Version = packet.Version;
            Length = packet.Lenght;

            if (packet.IpPacket.Version == IPVersion.IPv4)
            {
                var ipv4 = packet.IpPacket.Extract<IPv4Packet>();
                InternetHeaderLength = ipv4.HeaderLength.ToString();
                TypeOfService = ipv4.TypeOfService.ToString();
                FragmentOffset = ipv4.FragmentOffset.ToString();
                TimeToLive = ipv4.TimeToLive.ToString();
                PayloadLength = ipv4.PayloadLength.ToString();
                HeaderChecksum = ipv4.Checksum.ToString();
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