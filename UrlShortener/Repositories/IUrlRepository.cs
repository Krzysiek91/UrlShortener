namespace UrlShortener.Repositories
{
    public interface IUrlRepository
    {
        public Task<string> GetUrlByIdAsync(long id);
        public Task<long> GetIdByUrlAsync(string url);
        public Task<long> SaveUrlAsync(string url);
    }
}
