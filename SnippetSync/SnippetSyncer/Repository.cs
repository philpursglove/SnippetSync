using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;

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

        public List<GithubFile> GetFilesListFromRepo()
        {
            var files = ApiUrl.AppendPathSegment("/contents").GetJsonListAsync().Result;

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
