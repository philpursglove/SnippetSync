using NUnit.Framework;
using System.IO;
using System.Linq;

namespace SnippetSyncer.Tests
{
    [TestFixture]
    public class FullTest
    {
        [Test]
        public void End_To_End_Test()
        {
            SnippetSyncer syncer = new SnippetSyncer();
            string path = Path.Join(Path.GetTempPath(), "SnippetSync");
            string url = "https://github.com/philpursglove/CSharpSnippets";

            syncer.SyncSnippetFolder(path, url);

            Assert.That(Directory.GetFiles(path).Count, Is.EqualTo(4));
        }

    }
}