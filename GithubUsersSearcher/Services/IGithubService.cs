using GithubUsersSearcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubUsersSearcher.Services
{
    internal interface IGithubService
    {
        Task<GithubUser> GetUserAsync(string username);
    }
}
