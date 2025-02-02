using GithubUsersSearcher.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GithubUsersSearcher.Services
{
    public class GithubService : IGithubService
    {
        private readonly HttpClient _httpClient;

        public GithubService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GithubUsersSearcher");
        }

        public async Task<GithubUser> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("username cannot be null or empty", nameof(username));
            }

            string url = $"https://api.github.com/users/{username}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GithubUser>(content);

            return user;
        }

        public async Task<List<GithubRepository>> GetUserReposAsync(string reposUrl)
        {
            var response = await _httpClient.GetAsync(reposUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching repositories: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var reposJson = JArray.Parse(content);

            var repos = new List<GithubRepository>();
            foreach (var repo in reposJson)
            {
                repos.Add(new GithubRepository
                {
                    Name = repo["name"]?.ToString(),
                    Description = repo["description"]?.ToString(),
                    StargazersCount = repo["stargazers_count"]?.ToObject<int>() ?? 0,
                    HtmlUrl = repo["html_url"]?.ToString()
                });
            }

            repos.Sort((x, y) => y.StargazersCount.CompareTo(x.StargazersCount));
            return repos.Take(5).ToList();
        }
    }
}