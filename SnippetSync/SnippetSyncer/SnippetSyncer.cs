using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace SnippetSyncer
{
    public class SnippetSyncer
    {
        // SyncSnippets

        public void SyncSnippetFolder(string folderPath, string RepoUrl)
        {
            DateTime localFileTimestamp = GetTimestampFromLocalFile(folderPath);

            Repository repo = new Repository(RepoUrl);

            DateTime repoTimestamp = repo.LastUpdated();

            if (repoTimestamp > localFileTimestamp)
            {
                ClearFolder(folderPath);
                List<GithubFile> repoFiles = repo.GetFilesListFromRepo();
                foreach (GithubFile file in repoFiles)
                {
                    DownloadFile(file, folderPath);
                }
                LocalUpdateFile updateFile = new LocalUpdateFile { LastUpdated = DateTime.Now };
                SaveTimestampFile(folderPath, updateFile);
            }
        }

        public void SaveTimestampFile(string folderPath, LocalUpdateFile updateFile)
        {
            string updateJson = JsonSerializer.Serialize<LocalUpdateFile>(updateFile);
            string filePath = Path.Join(folderPath, "SnippetSync.json");
            File.WriteAllText(filePath, updateJson);
        }

        public DateTime GetTimestampFromLocalFile(string folderPath)
        {
            string filePath = Path.Join(folderPath, "SnippetSync.json");

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

        public void DownloadFile(GithubFile file, string folderPath)
        {
            WebClient client = new WebClient();
            client.DownloadFile(file.download_url, Path.Join(folderPath, file.name));
        }
    }
}
