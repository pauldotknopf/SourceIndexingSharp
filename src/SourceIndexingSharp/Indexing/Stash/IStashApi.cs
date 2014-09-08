namespace SourceIndexingSharp.Indexing.Stash
{
    public interface IStashApi
    {
        void ExtractSource(string destination, string host, string project, string repository, string file, string commit, StashCredentials credentials);
    }
}
