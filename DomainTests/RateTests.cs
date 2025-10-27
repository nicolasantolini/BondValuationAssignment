using Domain;
using Domain.Exceptions;

namespace DomainTests
{
    public class RateTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Parse_WithNullOrWhiteSpaceString_ReturnsZeroRate(string rateString)
        {
            // Act
            var rate = Rate.Parse(rateString);

            // Assert
            Assert.Equal(0m, rate.Value);
        }

        [Theory]
        [InlineData("50", 0.50d)]
        [InlineData("50%", 0.50d)]
        [InlineData(" 50% ", 0.50d)]
        [InlineData("7.5", 0.075d)]
        [InlineData("7.5%", 0.075d)]
        [InlineData(" 7.5% ", 0.075d)]
        public void Parse_WithValidRateString_ReturnsCorrectRate(string rateString, decimal expectedValue)
        {
            // Act
            var rate = Rate.Parse(rateString);

            // Assert
            Assert.Equal(expectedValue, rate.Value);
        }

        [Fact]
        public void Parse_WithInvalidRateString_ThrowsInvalidBondDataException()
        {
            // Arrange
            var invalidRateString = "not-a-rate";

            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() => Rate.Parse(invalidRateString));
            Assert.Equal("Invalid rate format.", exception.Message);
            Assert.Equal(nameof(Rate), exception.FieldName);
            Assert.Equal(invalidRateString, exception.InvalidValue);
        }

        [Fact]
        public void Parse_WithInvalidRateStringAndBondId_ThrowsExceptionWithDetails()
        {
            // Arrange
            var invalidRateString = "not-a-rate";
            var bondId = "BOND-123";

            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() => Rate.Parse(invalidRateString, bondId));
            Assert.Equal(bondId, exception.BondId);
        }
    }
}
