using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubUsersSearcher.Models
{
    public class GithubRepository
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StargazerCount { get; set; }
    }
}