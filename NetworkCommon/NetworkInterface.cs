using NetworkCommon.Messages;
using PacketDotNet;
using Prism.Events;
using SharpPcap;
using SharpPcap.LibPcap;
using System.Diagnostics;

namespace NetworkCommon
{
    public class NetworkInterface
    {
        private const int ReadTimeoutMilliseconds = 1000;
        private readonly IEventAggregator _eventAggregator;
        private int _packageCounter;
        private Stopwatch _stopwatch;

        public string Name { get; set; }

        public LibPcapLiveDevice Device { get; set; }        

        public NetworkInterface(IEventAggregator eventAggregator, LibPcapLiveDevice device, string name)
        {
            _eventAggregator = eventAggregator;
            _stopwatch = new Stopwatch();

            Device = device;
            Name = name;
        }
        
        public void StartCapture()
        {
            _packageCounter = 1;
            _stopwatch.Restart();            

            if (Device is LibPcapLiveDevice livePcapDevice)
            {
                Device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);

                livePcapDevice.Open(DeviceMode.Normal, ReadTimeoutMilliseconds);
            }
            
            Device.StartCapture();
        }
                
        public void StopCapture()
        {
            _stopwatch.Stop();

            Device.StopCapture();
        }

        private void Device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            var ipPacket = packet.Extract<IPPacket>();

            if (ipPacket != null)
            {
                var newPacket = new Models.Packet(_packageCounter, _stopwatch.Elapsed, ipPacket);
            
                _eventAggregator.GetEvent<SendPacketMessage>().Publish(newPacket);
                
                _packageCounter++;
            }
        }

        public override string ToString()
            => Name;
    }
}