using FluentAssertions;
using UrlShortener.Services;

namespace UrlShortener.Tests.Services
{
    public class EncodingServiceTest
    {
        private EncodingService _service;

        public EncodingServiceTest()
        {
            _service = new EncodingService();
        }

        [Fact]
        public void ShouldBeAssignableToIEncodingService()
        {
            typeof(IEncodingService).Should().BeAssignableTo<IEncodingService>();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(10, "a")]
        [InlineData(100, "2s")]
        [InlineData(3000, "2bc")]
        [InlineData(5000000, "2z60w")]
        [InlineData(9000000000000000000, "1wdllqhza58g0")]
        public void EncodeIntigerToString_GivenValidId_ShouldReturnExpectedValue(long id, string expectedString)
        {
            var result = _service.EncodeIntigerToString(id);
            result.Should().Be(expectedString);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(10, "a")]
        [InlineData(100, "2s")]
        [InlineData(3000, "2bc")]
        [InlineData(5000000, "2z60w")]
        [InlineData(9000000000000000000, "1wdllqhza58g0")]
        public void DecodeStringToIntiger_GivenValidString_ShouldReturnExpectedValue(long expectedId, string shortString)
        {
            var result = _service.DecodeStringToIntiger(shortString);
            result.Should().Be(expectedId);
        }
    }
}
