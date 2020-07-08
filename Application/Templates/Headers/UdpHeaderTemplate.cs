using Application.Templates.Abstractions;
using PacketDotNet;

namespace Application.Templates.Headers
{
    public class UdpHeaderTemplate : IPacketTemplate
    {
        public string SourcePort { get; set; }
        public string DestinationPort { get; set; }
        public string Length { get; set; }
        public string Checksum { get; set; }
        
        public UdpHeaderTemplate(UdpPacket packet)
        {
            SourcePort = packet.SourcePort.ToString();
            DestinationPort = packet.DestinationPort.ToString();
            Length = packet.Length.ToString();
            Checksum = packet.Checksum.ToString();
        }
    }
}