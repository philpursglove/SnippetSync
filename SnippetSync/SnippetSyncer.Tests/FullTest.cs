using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SnippetSyncer.Tests
{
    [TestFixture]
    public class FullTest
    {
        [Test]
        public async Task End_To_End_Test()
        {
            SnippetSyncer syncer = new SnippetSyncer();
            string path = Path.Combine(Path.GetTempPath(), "SnippetSync");
            string url = "https://github.com/philpursglove/CSharpSnippets";

            await syncer.SyncSnippetFolder(path, url);

            Assert.That(Directory.GetFiles(path).Count, Is.EqualTo(4));
        }

    }
}