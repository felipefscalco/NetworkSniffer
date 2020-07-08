using Application.Templates.Abstractions;
using Application.Templates.Headers;
using PacketDotNet;
using System.Collections.ObjectModel;

namespace Application.Templates.Packets
{
    public class IgmpPacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; }

        public IgmpPacketTemplate(IgmpV2Packet packet)
        {
            PacketContent = new ObservableCollection<IPacketTemplate>
            {
                new IgmpHeaderTemplate(packet)
            };
        }
    }
}