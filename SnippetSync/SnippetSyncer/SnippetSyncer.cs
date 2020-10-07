using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SnippetSyncer
{
    public class SnippetSyncer
    {
        // SyncSnippets

        public async Task SyncSnippetFolder(string folderPath, string RepoUrl)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            DateTime localFileTimestamp = GetTimestampFromLocalFile(folderPath);

            Repository repo = new Repository(RepoUrl);

            DateTime repoTimestamp = await repo.LastUpdated();

            if (repoTimestamp > localFileTimestamp)
            {
                ClearFolder(folderPath);
                List<GithubFile> repoFiles = await repo.GetFilesListFromRepo();
                foreach (GithubFile file in repoFiles)
                {
                    await DownloadFile(file, folderPath);
                }
                LocalUpdateFile updateFile = new LocalUpdateFile { LastUpdated = DateTime.Now };
                SaveTimestampFile(folderPath, updateFile);
            }
        }

        public void SaveTimestampFile(string folderPath, LocalUpdateFile updateFile)
        {
            string updateJson = JsonSerializer.Serialize<LocalUpdateFile>(updateFile);
            string filePath = Path.Combine(folderPath, "SnippetSync.json");
            File.WriteAllText(filePath, updateJson);
        }

        public DateTime GetTimestampFromLocalFile(string folderPath)
        {
            string filePath = Path.Combine(folderPath, "SnippetSync.json");

            if (!File.Exists(filePath))
            {
                return DateTime.MinValue;
            }
            string updateJson = File.ReadAllText(filePath);
            LocalUpdateFile updateFile = JsonSerializer.Deserialize<LocalUpdateFile>(updateJson);

            return updateFile.LastUpdated;
        }

        public void ClearFolder(string folderPath)
        {
            DirectoryInfo info = new DirectoryInfo(folderPath);

            FileInfo[] files = info.GetFiles();

            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        public async Task DownloadFile(GithubFile file, string folderPath)
        {
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(file.download_url), Path.Combine(folderPath, file.name));
        }
    }
}
