using Application.Templates.Abstractions;
using Application.Templates.Headers;
using PacketDotNet;
using System.Collections.ObjectModel;

namespace Application.Templates.Packets
{
    public class TcpPacketTemplate : IPacketTemplate
    {
        public ObservableCollection<IPacketTemplate> PacketContent { get; }

        public TcpPacketTemplate(TcpPacket packet)
        {
            PacketContent = new ObservableCollection<IPacketTemplate>
            {
                new TcpHeaderTemplate(packet)
            };
        }
    }
}