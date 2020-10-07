using System;
using System.Threading.Tasks;
using SnippetSyncer;

namespace SnippetSync
{
    public class SnippetSyncService : ISnippetSyncService, IAsyncSnippetSyncService
    {
        public async Task SyncSnippets()
        {
            SnippetSyncer.SnippetSyncer syncer = new SnippetSyncer.SnippetSyncer();

            await syncer.SyncSnippetFolder(@"C:\Temp", "https://github.com/philpursglove/charpsnippets");
        }
    }
}
