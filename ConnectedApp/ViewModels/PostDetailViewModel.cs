using System;
using ConnectedApp.Models;
using ConnectedApp.Services.Interfaces;
using Prism.Navigation;

namespace ConnectedApp.ViewModels
{
    public class PostDetailViewModel : ViewModelBase
    {
        private readonly IFakeService _fakeService;

        private Post _post;
        public Post Post
        {
            get { return _post; }
            set { SetProperty(ref _post, value); }
        }

        public PostDetailViewModel(INavigationService navigationService, IFakeService fakeService) : base(navigationService)
        {
            _fakeService = fakeService;
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            string postId = parameters["id"] as string;
            Post = await _fakeService.GetPost(postId);
        }
    }
}