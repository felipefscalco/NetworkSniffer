using System.Collections.ObjectModel;

namespace NetworkCommon.Interfaces
{
    public interface INetworkHelper
    {
        ObservableCollection<NetworkInterface> GetAvaliableInterfaces();
    }
}