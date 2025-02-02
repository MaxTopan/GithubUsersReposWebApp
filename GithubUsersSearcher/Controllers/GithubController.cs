using GithubUsersSearcher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GithubUsersSearcher.Controllers
{
    public class GithubController : Controller
    {
        private readonly GithubService _githubService;
        public GithubController(GithubService githubService)
        {
            _githubService = githubService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "Username cannot be empty.");
                return View("Index");
            }

            // ask the service to make the API call + process data
            var user = await _githubService.GetUserAsync(username);
            if (user == null | user.Username == null)
            {
                ModelState.AddModelError("username", "User not found.");
                return View("Index");
            }

            user.GithubRepositories = await _githubService.GetUserReposAsync(user.ReposUrl);

            // return the results view
            return View("Results", user);
        }
    }
}