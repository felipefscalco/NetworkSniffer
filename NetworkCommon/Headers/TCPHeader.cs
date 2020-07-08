using PacketDotNet;

namespace NetworkCommon.Headers
{
    public class TCPHeader
    {
        TcpPacket _tcpPacket;
        
        public TCPHeader(TcpPacket tcpPacket)
        {
            _tcpPacket = tcpPacket;
        }

        public string SourcePort
        {
            get
            {
                return _tcpPacket.SourcePort.ToString();
            }
        }

        public string DestinationPort
        {
            get
            {
                return _tcpPacket.DestinationPort.ToString();
            }
        }

        public string SequenceNumber
        {
            get
            {
                return _tcpPacket.SequenceNumber.ToString();
            }
        }

        public string AcknowledgementNumber
        {
            get
            {
                return _tcpPacket.AcknowledgmentNumber.ToString();
            }
        }

        public string HeaderLength
        {
            get
            {
                return _tcpPacket.DataOffset.ToString();
            }
        }

        public string WindowSize
        {
            get
            {
                return _tcpPacket.WindowSize.ToString();
            }
        }

        public string UrgentPointer
        {
            get
            {
                return _tcpPacket.UrgentPointer.ToString();
            }
        }

        public string Flags
        {
            get
            {
                return  string.Empty;
                    //"\r\n\t\tUrgente: " + (_tcpPacket.Urg ? "setado" : "não setado") +
                    //    "\r\n\t\tACK: " + (_tcpPacket.Ack ? "setado" : "não setado") +
                    //    "\r\n\t\tPSH: " + (_tcpPacket.Psh ? "setado" : "não setado") +
                    //    "\r\n\t\tRST: " + (_tcpPacket.Rst ? "setado" : "não setado") +
                    //    "\r\n\t\tSYN: " + (_tcpPacket.Syn ? "setado" : "não setado") +
                    //    "\r\n\t\tFIN: " + (_tcpPacket.Fin ? "setado" : "não setado") +
                    //    "\r\n\t\tECN: " + (_tcpPacket.ECN ? "setado" : "não setado") +
                    //    "\r\n\t\tCWR: " + (_tcpPacket.CWR ? "setado" : "não setado") +
                    //    "\r\n\t\tNS: " + (_tcpPacket.NS ? "setado" : "não setado");
            }
        }

        public string Checksum
        {
            get
            {
                return string.Format("0x{0:x2}", _tcpPacket.Checksum);
            }
        }
    }
}