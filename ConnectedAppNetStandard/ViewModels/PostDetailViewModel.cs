using System;
using ConnectedAppNetStandard.Models;
using ConnectedAppNetStandard.Services.Interfaces;
using Prism.Navigation;

namespace ConnectedAppNetStandard.ViewModels
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

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            string postId = parameters["id"] as string;
            _fakeService.GetPost(postId).Subscribe(item => Post = item);
        }
    }
}