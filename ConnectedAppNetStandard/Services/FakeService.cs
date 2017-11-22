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
using Polly.Wrap;

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

        private Polly.CircuitBreaker.CircuitBreakerPolicy _circuitBreakerPolicy;
        private Policy<List<Post>> _policy;

        //Cache posts for 30 sec
        private readonly TimeSpan _cachedPostsTime = new TimeSpan(0, 0, 15);
        //Cache detail of 30 sec
        private readonly TimeSpan _cachedPostTime = new TimeSpan(0, 0, 15);

        private const string CacheKeyPosts = "posts";

        public FakeService(IFakeAPI fakeAPI)
        {
            _fakeAPI = fakeAPI;
            ((FakeAPI)_fakeAPI).Retry = 0;

            _cache = BlobCache.LocalMachine;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 2, durationOfBreak: TimeSpan.FromSeconds(60));

            _policy = Policy<List<Post>>
                .Handle<Exception>()
                .FallbackAsync<List<Post>>(fallbackAction: async (System.Threading.CancellationToken arg) =>
                {
                    System.Diagnostics.Debug.WriteLine("Fallback call");
                    return await _fakeAPI.GetPostsFallback();
                })
                .WrapAsync(_circuitBreakerPolicy);
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
            var result = await _policy.ExecuteAsync(async () => await _fakeAPI.GetPosts());

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