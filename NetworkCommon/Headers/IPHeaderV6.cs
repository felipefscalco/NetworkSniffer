using NetworkCommon.Interfaces;
using System.IO;
using System.Net;

namespace NetworkCommon.Headers
{
    public class IPHeaderV6 : IIpHeader
    {
        private uint _versionTrafficClassAndFlowLabel;

        private ushort _payloadLength;

        private byte _nextHeader;
        private byte _hopLimit;

        private byte[] _sourceAddress = new byte[16];
        private byte[] _destinationAddress = new byte[16];

        public IPHeaderV6(BinaryReader binaryReader)
        {
            _versionTrafficClassAndFlowLabel = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            _payloadLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            _nextHeader = binaryReader.ReadByte();

            _hopLimit = binaryReader.ReadByte();

            for(int i = 0; i < 16; i++)
            {
                _sourceAddress[i] = binaryReader.ReadByte();
            }

            SourceAddress = new IPAddress(_sourceAddress);

            for (int i = 0; i < 16; i++)
            {
                _destinationAddress[i] = binaryReader.ReadByte();
            }

            DestinationAddress = new IPAddress(_destinationAddress);
        }

        public string Version
        {
            get
            {
                return "IP v6";
            }
        }

        public string TrafficClass
        {
            get
            {
                uint trafficClass = _versionTrafficClassAndFlowLabel << 4;
                trafficClass >>= 24;

                if (trafficClass == 0)
                {
                    return "Nenhum tráfego específico";
                }
                else if (trafficClass == 1)
                {
                    return "Dados de segundo plano";
                }
                else if (trafficClass == 2)
                {
                    return "Tráfego de dados não atendido";
                }
                else if (trafficClass == 3)
                {
                    return "Reservado";
                }
                else if (trafficClass == 4)
                {
                    return "Tráfego de dados pesado atendido";
                }
                else if (trafficClass == 5)
                {
                    return "Reservado";
                }
                else if (trafficClass == 6)
                {
                    return "Tráfego interativo";
                }
                else if (trafficClass == 7)
                {
                    return "Tráfego de controle";
                }
                else
                {
                    return "Desconhecido";
                }
            }
        }

        public string FlowLabel
        {
            get
            {
                uint flowLabel = _versionTrafficClassAndFlowLabel << 12;
                flowLabel >>= 12;

                return string.Format("0x{0:x2}",flowLabel);
            }
        }
        
        public string PayloadLength
        {
            get
            {
                return _payloadLength.ToString();
            }
        }

        public string NextHeader
        {
            get
            {
                if(_nextHeader == 6)
                {
                    return "TCP";
                }
                else if(_nextHeader == 17)
                {
                    return "UDP";
                }
                else
                {
                    return "Desconhecido";
                }
            }
        }

        public string HopLimit
        {
            get
            {
                return _hopLimit.ToString();
            }
        }

        public IPAddress SourceAddress { get; }

        public IPAddress DestinationAddress { get; }
    }
}