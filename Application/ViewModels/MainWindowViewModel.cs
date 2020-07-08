using Application.Templates.Abstractions;
using Application.Templates.Packets;
using NetworkCommon;
using NetworkCommon.Interfaces;
using NetworkCommon.Messages;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Packet = NetworkCommon.Models.Packet;

namespace Application.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly INetworkHelper _networkHelper;
        private ObservableCollection<Packet> _packetList;
        private ObservableCollection<NetworkInterface> _avaliableInterfaces;
        private NetworkInterface _selectedInterface;
        private Packet _selectedPacket;
        private ObservableCollection<IPacketTemplate> _packetTemplates;
        private bool _isStartCaptureEnabled;
        private bool _hasCaptureStarted;
        private bool _isFilterEnabled;
        private bool _isResetEnabled;
        private bool _isClearEnabled;
        private string _filterProperty;
        private string _filterText;
        private object _packetLock = new object();

        public ObservableCollection<Packet> PacketList
        {
            get => _packetList;
            set => SetProperty(ref _packetList, value);
        }

        public ObservableCollection<NetworkInterface> AvaliableInterfaces
        {
            get => _avaliableInterfaces;
            set => SetProperty(ref _avaliableInterfaces, value);
        }

        public ObservableCollection<IPacketTemplate> PacketTemplates
        {
            get => _packetTemplates;
            set => SetProperty(ref _packetTemplates, value);
        }

        public NetworkInterface SelectedInterface
        {
            get => _selectedInterface;
            set => SetProperty(ref _selectedInterface, value, () => UpdateStartCaptureState(value != null));
        }

        public Packet SelectedPacket
        {
            get => _selectedPacket;
            set => SetProperty(ref _selectedPacket, value);
        }

        public bool IsStartCaptureEnabled
        {
            get => _isStartCaptureEnabled;
            set => SetProperty(ref _isStartCaptureEnabled, value);
        }

        public bool HasCaptureStarted
        {
            get => _hasCaptureStarted;
            set => SetProperty(ref _hasCaptureStarted, value);
        }
        
        public bool IsFilterEnabled
        {
            get => _isFilterEnabled;
            set => SetProperty(ref _isFilterEnabled, value);
        }
        
        public bool IsResetEnabled
        {
            get => _isResetEnabled;
            set => SetProperty(ref _isResetEnabled, value);
        }
        
        public bool IsClearEnabled
        {
            get => _isClearEnabled;
            set => SetProperty(ref _isClearEnabled, value);
        }

        public string FilterProperty
        {
            get => _filterProperty;
            set => SetProperty(ref _filterProperty, value);
        }

         public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value);
        }

        public DelegateCommand StartCaptureCommand { get; private set; }
        public DelegateCommand StopCaptureCommand { get; private set; }
        public DelegateCommand SelectionChangedCommand { get; private set; }
        public DelegateCommand ApplyFilterCommand { get; private set; }
        public DelegateCommand ResetFilterCommand { get; private set; }
        public DelegateCommand ClearPacketListCommand { get; private set; }

        public MainWindowViewModel(IEventAggregator eventAggregator, INetworkHelper networkHelper)
        {
            _eventAggregator = eventAggregator;
            _networkHelper = networkHelper;

            CreateCommands();

            InitializeProperties();

            SubscribeEvents();
        }

        private void CreateCommands()
        {
            StartCaptureCommand = new DelegateCommand(() => StartCapture(), () => IsStartCaptureEnabled);
            StopCaptureCommand = new DelegateCommand(() => StopCapture());
            SelectionChangedCommand = new DelegateCommand(() => SelectionChanged());
            ApplyFilterCommand = new DelegateCommand(() => ApplyFilter());
            ResetFilterCommand = new DelegateCommand(() => ResetFilter());
            ClearPacketListCommand = new DelegateCommand(() => ClearPacketList());
        }
        private void InitializeProperties()
        {
            FilterProperty = "White";
            PacketList = new ObservableCollection<Packet>();
            BindingOperations.EnableCollectionSynchronization(PacketList, _packetLock);

            PacketTemplates = new ObservableCollection<IPacketTemplate>();

            AvaliableInterfaces = _networkHelper.GetAvaliableInterfaces();
            if (AvaliableInterfaces.Count > 0)
                SelectedInterface = AvaliableInterfaces.First();
        }

        private void SubscribeEvents()
        {
            _eventAggregator.GetEvent<SendPacketMessage>().Subscribe((packet) => AddPacket(packet));
        }

        private void StartCapture()
        {
            HasCaptureStarted = true;
            IsFilterEnabled = true;
            IsClearEnabled = true;

            SelectedInterface.StartCapture();
        }

        private void StopCapture()
        {
            HasCaptureStarted = false;
            SelectedInterface.StopCapture();
        }

        private void SelectionChanged()
        {
            PacketTemplates.Clear();

            if (SelectedPacket != null)
                PacketTemplates.Add(GenerateTemplate(SelectedPacket));
        }

        private void ApplyFilter()
        {            
            if (string.IsNullOrEmpty(FilterText))
                return;

            IsResetEnabled = true;

            if (IsFilterValid())
                FilterProperty = "LightGreen";
            else
                FilterProperty = "LightSalmon";
        }       

        private void ResetFilter()
        {
            IsResetEnabled = false;
            FilterText = string.Empty;
            FilterProperty = "White";
        }     

        private bool IsFilterValid()
        {
//            •	protocol filter: “protocol name” (e.g. TCP)
            //•	source IP address filter: “src=xxx.xxx.xxx.xxx” 
            //•	destination IP address filter: “dest=xxx.xxx.xxx.xxx” 
            //•	source port filter: “sp=port_number”
            //•	destination port filter: “dp=port_number”
            //•	length filter: “length>100”, “length<100”

            if (FilterText.Equals("protocol udp", StringComparison.InvariantCultureIgnoreCase) || FilterText.Equals("protocol tcp", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }   

        private void ClearPacketList()
        {
            PacketList.Clear();
            SelectedPacket = null;
        }

        private void UpdateStartCaptureState(bool val)
        {
            IsStartCaptureEnabled = val;
            StartCaptureCommand?.RaiseCanExecuteChanged();
        }

        private void AddPacket(Packet packet)
        {
            lock (PacketList)
                   PacketList.Add(packet);
        }

        private IPPacketTemplate GenerateTemplate(Packet selectedPacket)
        {
            return new IPPacketTemplate(selectedPacket);
        }
    }
}