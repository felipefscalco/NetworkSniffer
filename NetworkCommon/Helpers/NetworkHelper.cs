using NetworkCommon.Headers;
using NetworkCommon.Interfaces;
using NetworkCommon.Messages;
using PacketDotNet;
using Prism.Events;
using SharpPcap.LibPcap;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NetworkCommon.Helpers
{
    public class NetworkHelper : INetworkHelper
    {
        private readonly IEventAggregator _eventAggregator;

        public NetworkHelper(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _eventAggregator.GetEvent<ReadPacketMessage>().Subscribe((packet) => ReadPacket(packet));
        }

        public ObservableCollection<NetworkInterface> GetAvaliableInterfaces()
        {
            var avaliableInterfaces = new ObservableCollection<NetworkInterface>();

            foreach (var device in LibPcapLiveDeviceList.Instance)
            {
                if (!device.Interface.Addresses.Exists(a => a != null && a.Addr != null && a.Addr.ipAddress != null)) continue;
                var devInterface = device.Interface;
                var friendlyName = string.Concat(devInterface.FriendlyName, $" ({devInterface.Addresses.FirstOrDefault(a => a.Broadaddr != null && a.Broadaddr.ipAddress != null || a.Addr != null).Addr.ipAddress.MapToIPv4()})");
                var description = devInterface.Description;

                avaliableInterfaces.Add(new NetworkInterface(_eventAggregator, device, friendlyName));
            }

            return avaliableInterfaces;
        }

        private void ReadPacket(IPPacket packet)
        {
            switch (packet.Protocol)
            {
                case ProtocolType.IPv6HopByHopOptions:
                    break;
                case ProtocolType.Icmp:
                    break;
                case ProtocolType.Igmp: 
                    var igmpPacket = packet.Extract<IgmpV2Packet>();
                        if (igmpPacket != null)
                        {
                        }
                    break;
                case ProtocolType.Gpg:
                    break;
                case ProtocolType.IPv4:
                    break;
                    case ProtocolType.Tcp:
                        var tcpPacket = packet.Extract<TcpPacket>();
                        if (tcpPacket != null)
                        {
                            var tcpHeader = new TCPHeader(tcpPacket);
                        }
                    break;
                case ProtocolType.Egp:
                    break;
                case ProtocolType.Pup:
                    break;
                case ProtocolType.Udp:
                    var udpPacket = packet.Extract<UdpPacket>();
                    if (udpPacket != null)
                    {
                        var udpHeader = new UDPHeader(udpPacket);
                    }
                    break;
                case ProtocolType.Idp:
                    break;
                case ProtocolType.TP:
                    break;
                case ProtocolType.IPv6:
                    break;
                case ProtocolType.IPv6RoutingHeader:
                    break;
                case ProtocolType.IPv6FragmentHeader:
                    break;
                case ProtocolType.Rsvp:
                    break;
                case ProtocolType.Gre:
                    break;
                case ProtocolType.IPSecEncapsulatingSecurityPayload:
                    break;
                case ProtocolType.IPSecAuthenticationHeader:
                    break;
                case ProtocolType.IcmpV6:
                    var icmpv6Packet = packet.Extract<IcmpV6Packet>();
                    if (icmpv6Packet != null)
                    {
                        //MakeInformationUDP(udpHeader);
                    }
                    break;
                case ProtocolType.IPv6NoNextHeader:
                    break;
                case ProtocolType.IPv6DestinationOptions:
                    break;
                case ProtocolType.Ospf:
                    break;
                case ProtocolType.Mtp:
                    break;
                case ProtocolType.Encapsulation:
                    break;
                case ProtocolType.Pim:
                    break;
                case ProtocolType.CompressionHeader:
                    break;
                case ProtocolType.MobilityHeader:
                    break;
                case ProtocolType.HostIdentity:
                    break;
                case ProtocolType.Shim6:
                    break;
                case ProtocolType.Reserved253:
                    break;
                case ProtocolType.Reserved254:
                    break;
                case ProtocolType.Raw:
                    break;
                default:
                    break;
            }            
        }
    }
}