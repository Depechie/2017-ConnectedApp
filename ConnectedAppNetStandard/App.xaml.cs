﻿using ConnectedAppNetStandard.ViewModels;
using ConnectedAppNetStandard.Views;
using Prism.Unity;
using Xamarin.Forms;

namespace ConnectedApp
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

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
        }
    }
}