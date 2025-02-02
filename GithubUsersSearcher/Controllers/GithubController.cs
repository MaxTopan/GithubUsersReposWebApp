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
        public GithubController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username cannot be null or empty");
            }

            Console.WriteLine(username);

            // ask the service to make the API call + process data

            // return the results view

            return View();
        }
    }
}