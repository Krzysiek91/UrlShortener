using FakeItEasy;
using UrlShortener.Controllers;
using UrlShortener.Models;
using UrlShortener.Repositories;
using UrlShortener.Services;
using UrlShortener.Validators;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace UrlShortener.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;

        private readonly IUrlRepository _repo;
        private readonly IEncodingService _encodingService;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UrlViewModel> _validator;

        public HomeControllerTests()
        {
            _repo = A.Fake<IUrlRepository>();
            _encodingService = A.Fake<IEncodingService>();
            _validator = A.Fake<IValidator<UrlViewModel>>();
            _configuration = A.Fake<IConfiguration>();

            _homeController = new HomeController(_repo, _encodingService, _configuration, _validator);
        }

        [Fact]
        public void ShouldBeAssignableToControllerBase()
        {
            typeof(HomeController).Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public void IndexActionShouldReturnViewWithExpectedViewModel()
        {
            var result = ((ViewResult)_homeController.Index());
            var viewModel = (UrlViewModel)result.Model;
            viewModel.Should().NotBeNull();
            viewModel.Hidden.Should().Be(true);
        }

        [Fact]
        public void RedirectToOriginalShouldBeDecoratedWithGetAttribute()
        {
            var method = typeof(HomeController).GetMethod("RedirectToOriginal");
            method.Should().BeDecoratedWith<HttpGetAttribute>(atr => atr.Template == "{encodedString}");
        }

        [Fact]
        public async Task RedirectToOriginalShouldReturnRedirectActionResultAndMakeExpectedCalls()
        {
            A.CallTo(() => _encodingService.DecodeStringToIntiger("abcd")).Returns(5);
            A.CallTo(() => _repo.GetUrlByIdAsync(5)).Returns("https://www.google.com");

            var result = (RedirectResult)await _homeController.RedirectToOriginal("abcd");

            A.CallTo(() => _encodingService.DecodeStringToIntiger("abcd")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _repo.GetUrlByIdAsync(5)).MustHaveHappenedOnceExactly();

            result.Should().NotBeNull();
            result.Url.Should().Be("https://www.google.com");
        }

        [Fact]
        public async Task RedirectToOriginalWhenDecodeStringToIntigerThrowsExceptionShouldReturnErrorView()
        {
            A.CallTo(() => _encodingService.DecodeStringToIntiger(A<string>.Ignored)).Throws(new Exception());
            A.CallTo(() => _repo.GetUrlByIdAsync(5)).Returns("https://www.google.com");

            var result = (ViewResult)await _homeController.RedirectToOriginal("abcd");
            var viewModel = (ErrorViewModel)result.Model;

            result.Should().NotBeNull();
            viewModel.Should().NotBeNull();
            viewModel.ErrorMessage.Should().Be("Sorry we are unable to redirect your request. Please try again later");
        }

        [Fact]
        public async Task RedirectToOriginalWhenGetUrlByIdAsyncThrowsExceptionShouldReturnErrorView()
        {
            A.CallTo(() => _encodingService.DecodeStringToIntiger("abcd")).Returns(5);
            A.CallTo(() => _repo.GetUrlByIdAsync(5)).Throws(new Exception());

            var result = (ViewResult)await _homeController.RedirectToOriginal("abcd");
            var viewModel = (ErrorViewModel)result.Model;

            result.Should().NotBeNull();
            viewModel.Should().NotBeNull();
            viewModel.ErrorMessage.Should().Be("Sorry we are unable to redirect your request. Please try again later");
        }

        [Fact]
        public void ShortenUrlShouldBeDecoratedWithGetAttribute()
        {
            var method = typeof(HomeController).GetMethod("ShortenUrl");
            method.Should().BeDecoratedWith<HttpPostAttribute>();
        }

        [Fact]
        public async Task ShortenUrlGivenValidUrlShouldReturnViewAndMakeExpectedCalls()
        {
            var url = new UrlViewModel
            {
                OriginalUrl = "https://www.google.com"
            };

            A.CallTo(() => _repo.GetIdByUrlAsync("https://www.google.com")).Returns(0);
            A.CallTo(() => _repo.SaveUrlAsync("https://www.google.com")).Returns(152);
            A.CallTo(() => _encodingService.EncodeIntigerToString(152)).Returns("xyzhgf");

            var result = (ViewResult)await _homeController.ShortenUrl(url);

            A.CallTo(() => _repo.GetIdByUrlAsync("https://www.google.com")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _repo.SaveUrlAsync("https://www.google.com")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _encodingService.EncodeIntigerToString(152)).MustHaveHappenedOnceExactly();

            var viewModel = (UrlViewModel)result.Model;
            viewModel.Should().NotBeNull();
            viewModel.Hidden.Should().Be(false);
            viewModel.ShortUrl.Should().EndWith("/xyzhgf");
            viewModel.OriginalUrl.Should().Be("https://www.google.com");
        }

        [Fact]
        public async Task ShortenUrlGivenInvalidUrlShouldReturnErrorView()
        {
            var url = new UrlViewModel
            {
                OriginalUrl = "https://www.google.com"
            };

            A.CallTo(() => _validator.IsValid(url)).Throws(new ArgumentException("Validation Failed"));

            var result = (ViewResult)await _homeController.ShortenUrl(url);

            result.Should().NotBeNull();
            var viewModel = (ErrorViewModel)result.Model;
            viewModel.Should().NotBeNull();
            viewModel.ErrorMessage.Should().Be("Validation Failed");
        }
    }
}
