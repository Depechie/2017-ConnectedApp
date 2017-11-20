using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Akavache;
using ConnectedAppNetStandard.Models;
using ConnectedAppNetStandard.Services.Interfaces;
using Plugin.Connectivity;
using Polly;

namespace ConnectedAppNetStandard.Services
{
    /// <summary>
    /// Middleware service class
    /// Needed to incorporate caching before calling actual backend
    /// </summary>
    public class FakeService : IFakeService
    {
        private readonly IFakeAPI _fakeAPI;
        private readonly IBlobCache _cache;

        //Cache posts for 30 sec
        private readonly TimeSpan _cachedPostsTime = new TimeSpan(0, 0, 30);
        //Cache detail of 30 sec
        private readonly TimeSpan _cachedPostTime = new TimeSpan(0, 0, 30);

        private const string CacheKeyPosts = "posts";

        public FakeService(IFakeAPI fakeAPI)
        {
            _fakeAPI = fakeAPI;
            _cache = BlobCache.LocalMachine;
        }

        public Task<List<Comment>> GetCommentsByPost(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<Post> GetPost(string id) =>
            _cache.GetAndFetchLatest(id,
                async () => await GetPostAsync(id),
                offset => IsOffsetReached(offset, _cachedPostTime));

        public IObservable<List<Post>> GetPosts() =>
            _cache.GetAndFetchLatest(CacheKeyPosts,
                async () => await GetPostsAsync(),
                offset => IsOffsetReached(offset, _cachedPostsTime));

        private async Task<Post> GetPostAsync(string id)
        {
            var result = await _fakeAPI.GetPost(id);
            await Task.Delay(1000); //Fake longer network request, so we visually see the refresh happening on screen!

            //Add visual timestamp to title of a post, this way we see what time the data has been fechted
            result.Title = string.Concat(DateTime.Now.ToString("T", CultureInfo.InvariantCulture), " - ", result.Title);
            return result;
        }

        private async Task<List<Post>> GetPostsAsync()
        {
            ((FakeAPI)_fakeAPI).Retry = 0;

            var result = await Policy.Handle<WebException>().WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))).ExecuteAsync(async () => await _fakeAPI.GetPosts());

            await Task.Delay(1000); //Fake longer network request, so we visually see the refresh happening on screen!

            //Add visual timestamp to title of a post, this way we see what time the data has been fechted
            result = result.OrderBy(item => item.Id).Select(item =>
            {
                item.Title = string.Concat(DateTime.Now.ToString("T", CultureInfo.InvariantCulture), " - ", item.Title);
                return item;
            }).ToList();

            return result;
        }

        private bool IsOffsetReached(DateTimeOffset offset, TimeSpan timeSpan)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                TimeSpan elapsed = DateTimeOffset.Now - offset;
                return elapsed > timeSpan;
            }
            else
                return false;
        }
    }
}