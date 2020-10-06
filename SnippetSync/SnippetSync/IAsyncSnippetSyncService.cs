using Task = System.Threading.Tasks.Task;

namespace SnippetSync
{
    public interface IAsyncSnippetSyncService
    {
        Task SyncSnippets();
    }
}
