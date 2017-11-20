using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace ConnectedAppNetStandard.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        protected INavigationService _navigationService;

        public ViewModelBase()
        {
        }

        public ViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}