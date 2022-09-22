namespace UrlShortener.Models
{
    public class UrlViewModel
    {
        public string? OriginalUrl { get; set; }
        public string? ShortUrl { get; set; }
        public bool Hidden { get; set; }
    }
}
