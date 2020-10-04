using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnippetSyncer
{
    public class Repository
    {
        public string HttpUrl { get; set; }

        public Repository(string httpUrl)
        {
            HttpUrl = httpUrl;
        }

        private string ApiUrl()
        {
            string httpUrl = HttpUrl.ToLowerInvariant();
            httpUrl = httpUrl.Replace("https://github.com/", string.Empty);
            httpUrl = httpUrl.Replace("http://github.com/", string.Empty);

            return $"https://api.github.com/repos/{httpUrl}";
        }

        public async Task<DateTime> LastUpdated()
        {
            GithubRepository githubRepo = await ApiUrl()
                .WithBasicAuth(Environment.UserName, string.Empty)
                .WithHeader("User-Agent", "SnippetSync")
                .GetJsonAsync<GithubRepository>();
            return githubRepo.updated_at;
        }

        public async Task<List<GithubFile>> GetFilesListFromRepo()
        {
            var files = await new Url(ApiUrl()).AppendPathSegment("/contents")
                    .WithBasicAuth(Environment.UserName, string.Empty)
                    .WithHeader("User-Agent", "SnippetSync")
                    .GetJsonListAsync();

            List<GithubFile> githubFiles = new List<GithubFile>();

            for (int i = 0; i < files.Count; i++)
            {
                dynamic apifile = files[i];
                githubFiles.Add(new GithubFile { name = apifile.name, download_url = apifile.download_url });
            }

            return githubFiles;
        }
    }
}
