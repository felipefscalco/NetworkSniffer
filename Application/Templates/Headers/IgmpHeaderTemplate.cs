using Application.Templates.Abstractions;
using PacketDotNet;

namespace Application.Templates.Headers
{
    public class IgmpHeaderTemplate : IPacketTemplate
    {
        public string Type { get; set; }
        public string MaxResponseTime { get; set; }
        public string Checksum { get; set; }
        public string GroupAddress { get; set; }
        public string Length { get; set; }

        public IgmpHeaderTemplate(IgmpV2Packet packet)
        {
            Type = packet.Type.ToString();
            MaxResponseTime = packet.MaxResponseTime.ToString();
            Checksum = packet.Checksum.ToString();
            GroupAddress = packet.GroupAddress.ToString();
            Length = packet.TotalPacketLength.ToString();
        }
    }
}