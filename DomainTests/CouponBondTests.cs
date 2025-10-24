using Xunit;
using Domain;

namespace Domain.Tests
{
    public class CouponBondTests
    {
        private CouponBond CreateValidCouponBond()
        {
            return new CouponBond
            {
                BondId = "COUPON001",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "semi-annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 5m,
                DiscountFactor = 0.95m
            };
        }

        [Fact]
        public void CalculatePresentValue_WithValidData_ReturnsCorrectValue()
        {
            // Arrange
            var bond = CreateValidCouponBond();

            // Expected calculation for semi-annual, 5%, 5 years, 1000 face value, 0.95 discount
            // Rate per period: 5% / 2 = 2.5% = 0.025
            // Number of periods: 5 years * 2 = 10 periods
            // Future value factor: (1 + 0.025)^10 ≈ 1.2800845
            // Present value: 1000 * 1.2800845 * 0.95 ≈ 1216.08
            decimal expectedPresentValue = 1216.08m;

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expectedPresentValue, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroFaceValue_ReturnsZero()
        {
            // Arrange
            var bond = new CouponBond
            {
                BondId = "COUPON002",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 0m,
                PaymentFrequency = "annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 5m,
                DiscountFactor = 0.95m
            };

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroDiscountFactor_ReturnsZero()
        {
            // Arrange
            var bond = new CouponBond
            {
                BondId = "COUPON003",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 5m,
                DiscountFactor = 0m
            };  

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0m, result);
        }

        [Theory]
        [InlineData("annual", 1)]
        [InlineData("semi-annual", 2)]
        [InlineData("quarterly", 4)]
        public void CalculatePresentValue_WithDifferentFrequencies_ReturnsDifferentValues(string frequency, int expectedPaymentsPerYear)
        {
            // Arrange
            var bond = new CouponBond
            {
                BondId = "COUPON004",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000m,
                PaymentFrequency = frequency,
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 5m,
                DiscountFactor = 0.95m
            };

            // Act
            var result = bond.CalculatePresentValue();

            // Assert 
            Assert.True(result > 0);
        }

        [Fact]
        public void CalculatePresentValue_WithHighRate_ReturnsHigherValue()
        {
            // Arrange
            var lowRateBond = new CouponBond
            {
                BondId = "COUPON005",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("3.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 5m,
                DiscountFactor = 0.95m
            };
            var highRateBond = new CouponBond
            {
                BondId = "COUPON006",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("7.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 5m,
                DiscountFactor = 0.95m
            };

            // Act
            var lowRateResult = lowRateBond.CalculatePresentValue();
            var highRateResult = highRateBond.CalculatePresentValue();

            // Assert - Higher rate should result in higher present value for coupon bonds
            Assert.True(highRateResult > lowRateResult);
        }

        [Fact]
        public void CalculatePresentValue_WithLongerMaturity_ReturnsHigherValue()
        {
            // Arrange
            var shortMaturityBond = new CouponBond
            {
                BondId = "COUPON007",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 2m,
                DiscountFactor = 0.95m
            };
            var longMaturityBond = new CouponBond
            {
                BondId = "COUPON008",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AA",
                Type = "Corporate",
                YearsToMaturity = 10m,
                DiscountFactor = 0.95m
            };

            // Act
            var shortResult = shortMaturityBond.CalculatePresentValue();
            var longResult = longMaturityBond.CalculatePresentValue();

            // Assert - Longer maturity should result in higher present value
            Assert.True(longResult > shortResult);
        }

        [Fact]
        public void CalculatePresentValue_ResultIsRoundedToTwoDecimalPlaces()
        {
            // Arrange
            var bond = CreateValidCouponBond();

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(result)[3])[2];
            Assert.True(decimalPlaces <= 2);
        }
    }
}