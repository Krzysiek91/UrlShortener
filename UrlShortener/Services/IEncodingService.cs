namespace UrlShortener.Services
{
    public interface IEncodingService
    {
        string EncodeIntigerToString(long id);
        long DecodeStringToIntiger(string url);
    }
}
