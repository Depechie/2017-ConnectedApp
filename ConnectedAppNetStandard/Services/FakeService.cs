using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ConnectedAppNetStandard.Models;
using ConnectedAppNetStandard.Services.Interfaces;

namespace ConnectedAppNetStandard.Services
{
    /// <summary>
    /// Middleware service class
    /// Needed to incorporate caching before calling actual backend
    /// </summary>
    public class FakeService : IFakeService
    {
        private IFakeAPI _fakeAPI;

        public FakeService(IFakeAPI fakeAPI)
        {
            _fakeAPI = fakeAPI;
        }

        public Task<List<Comment>> GetCommentsByPost(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetPost(string id)
        {
            var result = await _fakeAPI.GetPost(id);

            //Add visual timestamp to title of a post, this way we see what time the data has been fechted
            result.Title = string.Concat(DateTime.Now.ToString("T", CultureInfo.InvariantCulture), " - ", result.Title);
            return result;
        }

        public async Task<List<Post>> GetPosts()
        {
            //TODO: Be sure to only call backend when needed
            var result = await _fakeAPI.GetPosts();

            //Add visual timestamp to title of a post, this way we see what time the data has been fechted
            result = result.Select(item =>
            {
                item.Title = string.Concat(DateTime.Now.ToString("T", CultureInfo.InvariantCulture), " - ", item.Title);
                return item;
            }).ToList();

            return result;
        }
    }
}