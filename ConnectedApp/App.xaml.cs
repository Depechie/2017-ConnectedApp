using ConnectedApp.Services;
using ConnectedApp.Services.Interfaces;
using ConnectedApp.ViewModels;
using ConnectedApp.Views;
using Prism.Unity;
using Xamarin.Forms;
using Microsoft.Practices.Unity;
using Akavache;
using System.Reactive.Linq;

namespace ConnectedApp
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            BlobCache.ApplicationName = "LazyDevCache";
            BlobCache.EnsureInitialized();

            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage, MainViewModel>();
            Container.RegisterTypeForNavigation<PostOverviewPage, PostOverviewViewModel>();
            Container.RegisterTypeForNavigation<PostDetailPage, PostDetailViewModel>();

            Container.RegisterType<IFakeAPI, FakeAPI>();
            Container.RegisterType<IFakeService, FakeService>();
        }

        protected override void OnSleep()
        {
            //https://github.com/akavache/Akavache/issues/342
            //BlobCache.Shutdown().Wait();
            BlobCache.LocalMachine.Flush().Wait();
            base.OnSleep();
        }
    }
}
