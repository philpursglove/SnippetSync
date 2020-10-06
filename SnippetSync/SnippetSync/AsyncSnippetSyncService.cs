using Task = System.Threading.Tasks.Task;

namespace SnippetSync
{
    public interface AsyncSnippetSyncService
    {
        Task SyncSnippets();
    }
}
