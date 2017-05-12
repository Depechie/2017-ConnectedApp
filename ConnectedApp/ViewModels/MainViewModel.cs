using System;
using Prism.Commands;
using Prism.Navigation;

namespace ConnectedApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Title => "PRISM";

        private DelegateCommand _showPostsCommand;
        public DelegateCommand ShowPostsCommand => _showPostsCommand ?? (_showPostsCommand = new DelegateCommand(async () => await _navigationService.NavigateAsync("PostOverviewPage")));

        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}