using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Repositories;
using UrlShortener.Services;
using UrlShortener.Validators;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlRepository _repo;
        private readonly IEncodingService _encodingService;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UrlViewModel> _validator;

        public HomeController(IUrlRepository urlRepo, IEncodingService encodingService, IConfiguration configuration, IValidator<UrlViewModel> validator)
        {
            _repo = urlRepo;
            _encodingService = encodingService;
            _configuration = configuration;
            _validator = validator;
        }

        public IActionResult Index()
        {
            return View(new UrlViewModel { Hidden = true });
        }

        [HttpGet("{encodedString}")]
        public async Task<IActionResult> RedirectToOriginal(string encodedString)
        {
            try
            {
                var id = _encodingService.DecodeStringToIntiger(encodedString);
                var url = await _repo.GetUrlByIdAsync(id);

                return Redirect(url);
            }
            catch
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Sorry we are unable to redirect your request. Please try again later" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl(UrlViewModel url)
        {
            try
            {
                _validator.IsValid(url);

                var originalUrl = url.OriginalUrl;
                var id = await _repo.GetIdByUrlAsync(originalUrl);

                if (id == 0)
                {
                    id = await _repo.SaveUrlAsync(originalUrl);
                }

                var encodedString = _encodingService.EncodeIntigerToString(id);
                var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                var domain = isDevelopment ? Request.Host.Value : _configuration.GetSection("ApplicationDomain").Value;
                var shortUrl = $"https://{domain}/{encodedString}";

                url.ShortUrl = shortUrl;

                return View("Index", url);
            }
            catch(ArgumentException ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ex.Message });
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            } 
        }
    }
}