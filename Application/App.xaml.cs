using Application.ViewModels;
using NetworkCommon.Helpers;
using NetworkCommon.Interfaces;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace Application
{
    public partial class App : PrismApplication
    {
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register <INetworkHelper, NetworkHelper>();
        }

        protected override Window CreateShell()
            => new MainWindow();
    }
}