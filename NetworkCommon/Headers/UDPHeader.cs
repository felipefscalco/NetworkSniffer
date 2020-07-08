using PacketDotNet;

namespace NetworkCommon.Headers
{
    public class UDPHeader
    {
        private UdpPacket _udpPacket;
        public UDPHeader(UdpPacket udpPacket)
        {
            _udpPacket = udpPacket;
        }

        public string SourcePort
        {
            get
            {
                return _udpPacket.SourcePort.ToString();
            }
        }

        public string DestinationPort
        {
            get
            {
                return _udpPacket.DestinationPort.ToString();
            }
        }

        public string Length
        {
            get
            {
                return _udpPacket.Length.ToString();
            }
        }

        public string Checksum
        {
            get
            {
                return string.Format("0x{0:x2}", _udpPacket.Checksum);
            }
        }
    }
}