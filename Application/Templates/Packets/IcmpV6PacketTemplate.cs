using Application.Templates.Abstractions;
using Application.Templates.Headers;
using PacketDotNet;
using System.Collections.ObjectModel;

namespace Application.Templates.Packets
{
    public class IcmpV6PacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; }

        public IcmpV6PacketTemplate(IcmpV6Packet packet)
        {
            PacketContent = new ObservableCollection<IPacketTemplate>
            {
                new Icmpv6HeaderTemplate(packet)
            };
        }
    }
}