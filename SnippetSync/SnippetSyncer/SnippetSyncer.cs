﻿using System;
using System.IO;
using System.Text.Json;

namespace SnippetSyncer
{
    public class SnippetSyncer
    {
        // SyncSnippets

        // SyncSnippetFolder(string folderPath, string RepoUrl)
        // {
        //      DateTime localFileTimestamp = GetTimeStampFromLocalFile(folderPath)
        //      DateTime repoTimestamp = GetRepoLastUpdateTime(repoUrl)
        //      
        //      if repoTimestamp > localFIleTimeStamp
        //      {
        //          ClearFolder(folderPath)
        //          List<GithubFile> repoFiles = GetFilesListFromRepo
        //          foreach file in repoFiles
        //          {
        //              DownloadFIle(file)
        //          }
        //      }
        // }

        // GetFilesListFromRepo(repoUrl)

        // DownloadFile(string fileDownloadUrl)

        public void SaveTimestampFile(string folderPath, LocalUpdateFile updateFile)
        {
            string updateJson = JsonSerializer.Serialize<LocalUpdateFile>(updateFile);
            string filePath = Path.Join(folderPath, "SnippetSync.json");
            File.WriteAllText(filePath, updateJson);
        }

        // GetRepoLastUpdateTime(string repoUrl)

        public DateTime GetTimestampFromLocalFile(string folderPath)
        {
            string filePath = Path.Join(folderPath, "SnippetSync.json");
            string updateJson = File.ReadAllText(filePath);
            LocalUpdateFile updateFile = JsonSerializer.Deserialize<LocalUpdateFile>(updateJson);

            return updateFile.LastUpdated;
        }

        // ClearFolder(string folderPath)
    }
}
