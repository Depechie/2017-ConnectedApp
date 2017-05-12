using System;
using System.Collections.ObjectModel;
using ConnectedApp.Models;
using ConnectedApp.Services.Interfaces;
using Prism.Navigation;

namespace ConnectedApp.ViewModels
{
    public class PostOverviewViewModel : ViewModelBase
    {
        private readonly IFakeService _fakeService;

        private ObservableCollection<Post> _posts = new ObservableCollection<Post>();
        public ObservableCollection<Post> Posts
        {
            get { return _posts; }
            set { SetProperty(ref _posts, value); }
        }

        private Post _selectedPost;
        public Post SelectedPost
        {
            get { return _selectedPost; }
            set
            {
                SetProperty(ref _selectedPost, value);
                if (value != null)
                    _navigationService.NavigateAsync($"PostDetailPage?id={_selectedPost.Id}");
            }
        }

        public IFakeService FakeService
        {
            get
            {
                return _fakeService;
            }
        }

        public PostOverviewViewModel(INavigationService navigationService, IFakeService fakeService) : base(navigationService)
        {
            _fakeService = fakeService;
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (Posts == null || Posts.Count == 0)
            {
                var result = await FakeService.GetPosts();
                Posts = new ObservableCollection<Post>(result);
            }
        }
    }
}