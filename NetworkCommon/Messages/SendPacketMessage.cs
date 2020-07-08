using Prism.Events;

namespace NetworkCommon.Messages
{
    public class SendPacketMessage : PubSubEvent<Models.Packet>
    {
    }
}