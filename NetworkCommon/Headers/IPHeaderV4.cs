using NetworkCommon.Interfaces;
using System;
using System.IO;
using System.Net;

namespace NetworkCommon.Headers
{
    public class IPHeaderV4 : IIpHeader
    {
        private byte _headerLength;
        private byte _differentiatedServices;
        private byte _timeToLive;
        private byte _protocol;

        private ushort _totalLength;
        private ushort _identification;
        private ushort _flagsAndOffset;

        private short _checksum;

        public IPHeaderV4(BinaryReader binaryReader)
        {
            try
            {
                _headerLength = binaryReader.ReadByte();
                _headerLength <<= 4;
                _headerLength >>= 4;
                _headerLength *= 4;

                _differentiatedServices = binaryReader.ReadByte();

                _totalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                _identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                _flagsAndOffset = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                _timeToLive = binaryReader.ReadByte();

                _protocol = binaryReader.ReadByte();

                _checksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                SourceAddress = new IPAddress((uint)(binaryReader.ReadInt32()));

                DestinationAddress = new IPAddress((uint)(binaryReader.ReadInt32()));
            }
            catch (Exception)
            {

            }
        }

        public string Version
        {
            get
            {
                return "IP v4";
            }
        }
        public string HeaderLength
        {
            get
            {
                return _headerLength.ToString();
            }
        }

        public string DifferentiatedServices
        {
            get
            {
                return string.Format("0x{0:x2} ({1})", _differentiatedServices, _differentiatedServices);
            }
        }

        public string TotalLength
        {
            get
            {
                return _totalLength.ToString();
            }
        }

        public string Identification
        {
            get
            {
                return _identification.ToString();
            }
        }

        public string Flags
        {
            get
            {
                int flags = _flagsAndOffset >> 13;
                if (flags == 2)
                {
                    return "Não fragmentado";
                }
                else if (flags == 1)
                {
                    return "Mais fragmentos";
                }
                else
                {
                    return flags.ToString();
                }
            }
        }

        public string FragmentOffset
        {
            get
            {
                int offset = _flagsAndOffset << 3;
                offset >>= 3;

                return offset.ToString();
            }
        }

        public string TimeToLive
        {
            get
            {
                return _timeToLive.ToString();
            }
        }

        public string Protocol
        {
            get
            {
                if (_protocol == 6)
                {
                    return "TCP";
                }
                else if (_protocol == 17)
                {
                    return "UDP";
                }
                else
                {
                    return "Desconhecido";
                }
            }
        }

        public string Checksum
        {
            get
            {
                return string.Format("0x{0:x2}", _checksum);
            }
        }

        public IPAddress SourceAddress { get; }

        public IPAddress DestinationAddress { get; }
    }
}