using GithubUsersSearcher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubUsersSearcher.Services
{
    public interface IGithubService
    {
        Task<GithubUser> GetUserAsync(string username);
        Task<List<GithubRepository>> GetUserReposAsync();
    }
}