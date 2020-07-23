using Application.Templates.Abstractions;
using Application.Templates.Packets;
using Application.Validations;
using NetworkCommon;
using NetworkCommon.Interfaces;
using NetworkCommon.Messages;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Packet = NetworkCommon.Models.Packet;

namespace Application.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator EventAggregator;
        private readonly INetworkHelper NetworkHelper;
        private readonly FilterValidation FilterValidator;
        private ObservableCollection<Packet> _packetList;
        private List<Packet> _backupPacketList;
        private ObservableCollection<NetworkInterface> _avaliableInterfaces;
        private ObservableCollection<IPacketTemplate> _packetTemplates;
        private NetworkInterface _selectedInterface;
        private Packet _selectedPacket;
        private bool _isStartCaptureEnabled;
        private bool _hasCaptureStarted;
        private bool _isFilterEnabled;
        private bool _isFilterActive;
        private bool _isResetEnabled;
        private bool _isClearEnabled;
        private bool _isComboBoxEnabled;
        private string _filterProperty;
        private string _filterText;
        private object _packetLock = new object();
        private object _filteredPacketLock = new object();

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

        public bool IsComboBoxEnabled
        {
            get => _isComboBoxEnabled;
            set => SetProperty(ref _isComboBoxEnabled, value);
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
            EventAggregator = eventAggregator;
            NetworkHelper = networkHelper;
            FilterValidator = new FilterValidation();

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
            IsComboBoxEnabled = true;
            FilterProperty = "White";
            
            _backupPacketList = new List<Packet>();
            BindingOperations.EnableCollectionSynchronization(_backupPacketList, _filteredPacketLock);

            PacketList = new ObservableCollection<Packet>();
            BindingOperations.EnableCollectionSynchronization(PacketList, _packetLock);

            PacketTemplates = new ObservableCollection<IPacketTemplate>();

            AvaliableInterfaces = NetworkHelper.GetAvaliableInterfaces();
            if (AvaliableInterfaces.Count > 0)
                SelectedInterface = AvaliableInterfaces.First();
        }

        private void SubscribeEvents()
        {
            EventAggregator.GetEvent<SendPacketMessage>().Subscribe((packet) => AddPacket(packet));
        }

        private void StartCapture()
        {
            IsComboBoxEnabled = false;
            HasCaptureStarted = true;
            IsFilterEnabled = true;
            IsClearEnabled = true;

            SelectedInterface.StartCapture();
        }

        private void StopCapture()
        {
            IsComboBoxEnabled = true;
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

            if (FilterValidator.IsFilterValid(FilterText))
            {
                _isFilterActive = true;
                FilterProperty = "LightGreen";
                PacketList.Clear();

                foreach (var item in FilterValidator.ProtocolFilters)
                {
                    PacketList.AddRange(_backupPacketList.Where(p => p.Protocol.Equals(item.ToString())));
                }
            }
            else
            {
                _isFilterActive = false;
                FilterProperty = "LightSalmon";
            }
        }

        private void ResetFilter()
        {
            _isFilterActive = false;
            IsResetEnabled = false;
            FilterText = string.Empty;
            FilterProperty = "White";

            PacketList.Clear();
            PacketList.AddRange(_backupPacketList);
        }       

        private void ClearPacketList()
        {
            PacketList.Clear();
            _backupPacketList.Clear();
            SelectedPacket = null;
        }

        private void UpdateStartCaptureState(bool val)
        {
            IsStartCaptureEnabled = val;
            StartCaptureCommand?.RaiseCanExecuteChanged();
        }

        private void AddPacket(Packet packet)
        {
            if (_isFilterActive)
            {
                if(FilterValidator.ShouldAddPackageToList(packet))
                    PacketList.Add(packet);
            }
            else
            {
                lock (PacketList)
                    PacketList.Add(packet);
            }

            lock (_backupPacketList)
                _backupPacketList.Add(packet);
        }

        private IPPacketTemplate GenerateTemplate(Packet selectedPacket)
        {
            return new IPPacketTemplate(selectedPacket);
        }
    }
}