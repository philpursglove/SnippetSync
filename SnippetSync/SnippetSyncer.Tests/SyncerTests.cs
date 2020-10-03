using NUnit.Framework;
using System.IO;
using SnippetSyncer;
using System;
using Flurl.Http.Testing;

namespace SnippetSyncer.Tests
{
    public class SyncerTests
    {
        HttpTest _httpTest;

        [SetUp]
        public void Setup()
        {
            FolderPath = Path.Join(Path.GetTempPath(), "SnippetSync");
            FilePath = Path.Join(FolderPath, "SnippetSync.json");

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            _httpTest = new HttpTest();
        }

        [TearDown]
        public void Teardown()
        {
            _httpTest.Dispose();
        }

        public string FolderPath { get; set; }

        public string FilePath { get; set; }

        [Test]
        public void SaveTimestampFile_Saves_File()
        {
            SnippetSyncer syncer = new SnippetSyncer();

            LocalUpdateFile updateFile = new LocalUpdateFile { LastUpdated = DateTime.Now };

            syncer.SaveTimestampFile(FolderPath, updateFile);

            Assert.That(File.Exists(FilePath));
        }

        [Test]
        public void Can_Read_Timestamp_From_File()
        {
            DateTime testTimestamp = new DateTime(2020, 7, 5, 12, 0, 0);
            LocalUpdateFile updateFile = new LocalUpdateFile { LastUpdated = testTimestamp };

            SnippetSyncer syncer = new SnippetSyncer();

            syncer.SaveTimestampFile(FolderPath, updateFile);

            DateTime localFileTimestamp = syncer.GetTimestampFromLocalFile(FolderPath);
            Assert.That(localFileTimestamp, Is.EqualTo(testTimestamp));
        }

        [Test]
        public void ClearFolder_Removes_All_Files_From_A_Folder()
        {
            File.WriteAllText(Path.Join(FolderPath, Path.GetRandomFileName()), "test");
            File.WriteAllText(Path.Join(FolderPath, Path.GetRandomFileName()), "test");
            File.WriteAllText(Path.Join(FolderPath, Path.GetRandomFileName()), "test");

            SnippetSyncer syncer = new SnippetSyncer();

            syncer.ClearFolder(FolderPath);

            Assert.That(Directory.GetFiles(FolderPath).Length, Is.EqualTo(0));
        }

        [Test]
        public void File_Is_Downloaded()
        {
            SnippetSyncer syncer = new SnippetSyncer();
            GithubFile file = new GithubFile() { name = "StringProperty.snippet", download_url = "https://raw.githubusercontent.com/philpursglove/CSharpSnippets/master/StringProperty.snippet" };
            syncer.DownloadFile(file, FolderPath);
            Assert.That(File.Exists(Path.Join(FolderPath, "StringProperty.snippet")));
        }

    }
}