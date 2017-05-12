using ConnectedApp.Services;
using ConnectedApp.Services.Interfaces;
using ConnectedApp.ViewModels;
using ConnectedApp.Views;
using Prism.Unity;
using Xamarin.Forms;
using Microsoft.Practices.Unity;

namespace ConnectedApp
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
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
    }
}
