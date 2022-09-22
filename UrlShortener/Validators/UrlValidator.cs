using UrlShortener.Models;

namespace UrlShortener.Validators
{
    public class UrlValidator : IValidator<UrlViewModel>
    {
        public void IsValid(UrlViewModel url)
        {
            if (url == null || string.IsNullOrEmpty(url.OriginalUrl) || !Uri.TryCreate(url.OriginalUrl, UriKind.Absolute, out _))
            {
                throw (new ArgumentException("Your input is invalid. Please provide absolute URL (including http/https protocol)."));
            };
        }
    }
}
