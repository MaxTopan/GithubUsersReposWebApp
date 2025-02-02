using GithubUsersSearcher.Models;
using System.Threading.Tasks;

namespace GithubUsersSearcher.Services
{
    public interface IGithubService
    {
        Task<GithubUser> GetUserAsync(string username);
        Task<GithubUser> GetUserReposAsync(string reposUrl);
    }
}