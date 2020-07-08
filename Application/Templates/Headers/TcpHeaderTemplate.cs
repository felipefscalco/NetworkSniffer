using Application.Templates.Abstractions;
using PacketDotNet;

namespace Application.Templates.Headers
{
    public class TcpHeaderTemplate : IPacketTemplate
    {
        public string SourcePort { get; }
        public string DestinationPort { get; }
        public string SequenceNumber { get; }
        public string AcknowledgmentNumber { get; }
        public string DataOffset { get; }
        public string Flags { get; }
        public string WindowSize { get; }
        public string Checksum { get; }
        public string UrgentPointer { get; }

        public TcpHeaderTemplate(TcpPacket packet)
        {
            SourcePort = packet.SourcePort.ToString();
            DestinationPort = packet.DestinationPort.ToString();
            SequenceNumber = packet.SequenceNumber.ToString();
            AcknowledgmentNumber = packet.AcknowledgmentNumber.ToString();
            DataOffset = packet.DataOffset.ToString();
            Flags = CreateFlagsString(packet.Flags);
            Checksum = packet.Checksum.ToString();
            UrgentPointer = packet.UrgentPointer.ToString();
        }

        private string CreateFlagsString(ushort flags)
        {
            string value = "(";

            if ((flags & 0x01) != 0)
            {
                value += "FIN, ";
            }
            if ((flags & 0x02) != 0)
            {
                value += "SYN, ";
            }
            if ((flags & 0x04) != 0)
            {
                value += "RST, ";
            }
            if ((flags & 0x08) != 0)
            {
                value += "PSH, ";
            }
            if ((flags & 0x10) != 0)
            {
                value += "ACK, ";
            }
            if ((flags & 0x20) != 0)
            {
                value += "URG";
            }
            value += ")";

            if (value == "()")
            {
                value = "";
            }
            else if (value.Contains(", )"))
            {
                value = value.Remove(value.Length - 3, 2);
            }

            return value;
        }
    }
}