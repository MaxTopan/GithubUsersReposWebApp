using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubUsersSearcher.Models
{
    public class GithubUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Username { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }
        public GithubRepository[] GithubRepositories { get; set; }
    }
}