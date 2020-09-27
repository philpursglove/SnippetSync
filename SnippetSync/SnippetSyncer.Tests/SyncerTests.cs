using NUnit.Framework;
using System.IO;
using SnippetSyncer;
using System;

namespace SnippetSyncer.Tests
{
    public class SyncerTests
    {

        public string FolderPath { get; set; }

        public string FilePath { get; set; }

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
        }

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
    }
}