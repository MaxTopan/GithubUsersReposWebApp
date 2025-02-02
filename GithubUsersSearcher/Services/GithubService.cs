using GithubUsersSearcher.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username cannot be null or empty", nameof(username));
            }

            string url = $"https://api.github.com/users/{username}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception($"User {username} not found");
                }
                throw new Exception($"Error fetching user data: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GithubUser>(content);

            return user;
        }

        public Task<List<GithubRepository>> GetUserReposAsync()
        {
            throw new NotImplementedException();
        }
    }
}