using Application.Templates.Abstractions;
using PacketDotNet;

namespace Application.Templates.Headers
{
    public class Icmpv6HeaderTemplate : IPacketTemplate
    {
        public string ICMPType { get; set; }
        public string Code { get; set; }
        public string Checksum { get; set; }
        public string Lenght { get; set; }

        public Icmpv6HeaderTemplate(IcmpV6Packet packet)
        {
            ICMPType = packet.Type.ToString();
            Code = packet.Code.ToString();
            Checksum = packet.Checksum.ToString();
            Lenght = packet.TotalPacketLength.ToString();
        }
    }
}