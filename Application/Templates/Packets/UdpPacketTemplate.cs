using Application.Templates.Abstractions;
using Application.Templates.Headers;
using PacketDotNet;
using System.Collections.ObjectModel;

namespace Application.Templates.Packets
{
    public class UdpPacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; }
        public UdpPacketTemplate(UdpPacket packet)
        {
            PacketContent = new ObservableCollection<IPacketTemplate>
            {
                new UdpHeaderTemplate(packet)
            };
        }
    }
}