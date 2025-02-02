using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubUsersSearcher.Models
{
    public class GithubRepository
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("id")]
        public int AuthorId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("stargazer_count")]
        public int StargazerCount { get; set; }
    }
}