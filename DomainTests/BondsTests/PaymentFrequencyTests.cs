using Domain;
using Domain.Exceptions;

namespace DomainTests.BondsTests
{
    public class BondPaymentFrequencyTests
    {
        [Theory]
        [InlineData("quarterly", 4)]
        [InlineData("semi-annual", 2)]
        [InlineData("annual", 1)]
        [InlineData("none", 0)]
        public void GetPaymentsPerYear_WithValidFrequency_ReturnsCorrectValue(string frequency, int expectedPayments)
        {
            // Arrange
            var bond = new TestBond
            {
                BondId = "BOND001",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000m,
                PaymentFrequency = frequency,
                Rating = "AA",
                Type = "Coupon",
                YearsToMaturity = 10m,
                DiscountFactor = 0.95m
            };

            // Act
            var paymentsPerYear = bond.GetPaymentsPerYear();

            // Assert
            Assert.Equal(expectedPayments, paymentsPerYear);
        }

        [Theory]
        [InlineData("monthly")]
        [InlineData("invalid")]
        [InlineData("")]
        [InlineData(null)]
        public void GetPaymentsPerYear_WithInvalidFrequency_ThrowsException(string invalidFrequency)
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = "BOND001",
                    Issuer = "Test Issuer",
                    Rate = Rate.Parse("5.00%"),
                    FaceValue = 1000m,
                    PaymentFrequency = invalidFrequency,
                    Rating = "AA",
                    Type = "Coupon",
                    YearsToMaturity = 10m,
                    DiscountFactor = 0.95m
                };
            });

            Assert.Equal(nameof(Bond.PaymentFrequency), exception.FieldName);
        }

        [Fact]
        public void PaymentFrequency_WhenSet_ValidatesImmediately()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var bond = new TestBond
                {
                    BondId = "BOND001",
                    Issuer = "Test Issuer",
                    Rate = Rate.Parse("5.00%"),
                    FaceValue = 1000m,
                    PaymentFrequency = "invalid-frequency",
                    Rating = "AA",
                    Type = "Coupon",
                    YearsToMaturity = 10m,
                    DiscountFactor = 0.95m
                };
            });
        }
    }
}
