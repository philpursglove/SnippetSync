using NUnit.Framework;
using System.IO;
using SnippetSyncer;

namespace SnippetSyncer.Tests
{
    public class SyncerTests
    {
        [Test]
        public void SaveTimestampFile_Saves_File()
        {
            string folderPath = Path.Join(Path.GetTempPath(), "SnippetSync");
            string filePath = Path.Join(folderPath, "SnippetSync.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            SnippetSyncer syncer = new SnippetSyncer();

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            syncer.SaveTimestampFile(folderPath);

            Assert.That(File.Exists(filePath));
        }
    }
}