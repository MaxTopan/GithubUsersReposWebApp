using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubUsersSearcher.Models
{
    public class GithubUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string HtmlUrl { get; set; }
        public string AvatarUrl { get; set; }
        public string Location { get; set; }
        public string ReposUrl { get; set; }
        public GithubRepository[] GithubRepositories { get; set; }
    }
}