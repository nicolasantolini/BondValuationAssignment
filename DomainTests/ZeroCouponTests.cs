using Domain;
using Xunit;

namespace Domain.Tests
{
    public class ZeroCouponBondTests
    {
        private ZeroCouponBond CreateValidZeroCouponBond()
        {
            return new ZeroCouponBond
            {
                BondId = "ZERO001",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.00%"), // 5% annual rate
                FaceValue = 1000.0,
                PaymentFrequency = "none", // Zero-coupon typically has no periodic payments
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10.0,
                DiscountFactor = 1.0 // No additional discount for simplicity
            };
        }

        [Fact]
        public void CalculatePresentValue_WithValidData_ReturnsCorrectValue()
        {
            // Arrange
            var bond = CreateValidZeroCouponBond();

            // Expected calculation: PV = (1 + 0.05)^10 × 1000 × 1.0
            // (1.05)^10 ≈ 1.628894626777441
            // 1000 × 1.628894626777441 ≈ 1628.89
            double expectedPresentValue = 1628.89;

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expectedPresentValue, result, 2);
        }

        [Fact]
        public void CalculatePresentValue_WithDiscountFactor_AppliesDiscountCorrectly()
        {
            // Arrange
            var bond = new ZeroCouponBond
            {
                BondId = "ZERO123",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.00%"), 
                FaceValue = 1000.0,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10.0,
                DiscountFactor = 0.9 // 10% discount
            };

            // Expected: (1.05)^10 × 1000 × 0.9 ≈ 1628.89 × 0.9 = 1466.01
            double expectedPresentValue = 1466.01;

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expectedPresentValue, result, 2);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroFaceValue_ReturnsZero()
        {
            // Arrange
            var bond = new ZeroCouponBond
            {
                BondId = "ZERO999",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.00%"), 
                FaceValue = 0.0,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10.0,
                DiscountFactor = 1.0
            };

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroDiscountFactor_ReturnsZero()
        {
            // Arrange
            var bond = new ZeroCouponBond
            {
                BondId = "ZERO000",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.00%"), 
                FaceValue = 1000.0,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10.0,
                DiscountFactor = 0.0
            };

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroYearsToMaturity_ReturnsFaceValueTimesDiscount()
        {
            // Arrange
            var bond = new ZeroCouponBond
            {
                BondId = "ZERO001",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.00%"),
                FaceValue = 1000.0,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 0.0,
                DiscountFactor = 0.95
            };

            // Expected: (1.05)^0 × 1000 × 0.95 = 1 × 1000 × 0.95 = 950.00
            double expectedPresentValue = 950.00;

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expectedPresentValue, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroInterestRate_ReturnsFaceValueTimesDiscount()
        {
            // Arrange
            var bond = new ZeroCouponBond
            {
                BondId = "ZERO002",
                Issuer = "US Treasury",
                Rate = Rate.Parse("0.00%"), // 0% annual rate
                FaceValue = 1000.0,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10.0,
                DiscountFactor = 0.9
            };

            // Expected: (1 + 0)^10 × 1000 × 0.9 = 1 × 1000 × 0.9 = 900.00
            double expectedPresentValue = 900.00;

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expectedPresentValue, result);
        }
    }
}