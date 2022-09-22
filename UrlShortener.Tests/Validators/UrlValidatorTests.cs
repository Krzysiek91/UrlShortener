using FluentAssertions;
using UrlShortener.Models;
using UrlShortener.Validators;

namespace UrlShortener.Tests.Validators
{
    public class UrlValidatorTests
    {
        [Fact]
        public void ShouldBeAssignableToIValidator()
        {
            typeof(UrlValidator).Should().BeAssignableTo<IValidator<UrlViewModel>>();
        }

        [Fact]
        public void IsValidGivenValidUrlShouldNotThrowException()
        {
            var url = new UrlViewModel
            {
                OriginalUrl = "https://www.google.com/"
            };

            var validator = new UrlValidator();

            Action method = () => validator.IsValid(url);
            method.Should().NotThrow();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("withoutprotocol.com")]
        public void IsValidGivenInvalidUrlShouldThrowException(string url)
        {
            var vm = new UrlViewModel
            {
                OriginalUrl = url
            };

            var validator = new UrlValidator();
            Action method = () => validator.IsValid(vm);
            method.Should().Throw<ArgumentException>();
        }
    }
}
