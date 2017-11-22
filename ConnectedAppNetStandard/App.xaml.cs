using ConnectedAppNetStandard.Services.Interfaces;
using ConnectedAppNetStandard.ViewModels;
using ConnectedAppNetStandard.Views;
using Prism.Unity;
using Xamarin.Forms;
using CommonServiceLocator;
using ConnectedAppNetStandard.Services;
using Unity;
using Akavache;
using System.Reactive.Linq;

namespace ConnectedApp
{
    public partial class App : PrismApplication
    {
        public IFakeService FakeService
        {
            get;
            private set;
        }

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

            Container.RegisterSingleton<IFakeService, FakeService>();
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