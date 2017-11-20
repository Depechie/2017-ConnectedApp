using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectedAppNetStandard.Models;
using Refit;

namespace ConnectedAppNetStandard.Services.Interfaces
{
    [Headers("Accept: application/json")]
    public interface IFakeAPI
    {
        [Get("/posts")]
        Task<List<Post>> GetPosts();

        [Get("/posts/{id}")]
        Task<Post> GetPost(string id);

        [Get("/posts/{id}/comments")]
        Task<List<Comment>> GetCommentsByPost(string id);
    }
}