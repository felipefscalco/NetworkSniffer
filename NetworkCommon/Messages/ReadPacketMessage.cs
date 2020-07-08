using PacketDotNet;
using Prism.Events;

namespace NetworkCommon.Messages
{
    public class ReadPacketMessage : PubSubEvent<IPPacket>
    {
    }
}