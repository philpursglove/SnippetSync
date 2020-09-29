using Flurl.Http;
using System;

namespace SnippetSyncer
{
    public class Repository
    {
        public string HttpUrl { get; set; }

        public Repository(string httpUrl)
        {
            HttpUrl = httpUrl;
        }

        private string ApiUrl
        {
            get
            {
                string httpUrl = HttpUrl.ToLowerInvariant();
                httpUrl = httpUrl.Replace("https://github.com/", string.Empty);
                httpUrl = httpUrl.Replace("http://github.com/", string.Empty);

                return $"https://api.github.com/repos/{httpUrl}";
            }
            set { }
        }

        public DateTime LastUpdated
        {
            get
            {
                GithubRepository githubRepo = ApiUrl.GetJsonAsync<GithubRepository>().Result;
                return githubRepo.updated_at;
            }
            private set { }
        }

    }
}
