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
                FaceValue = 1000m,
                PaymentFrequency = "none", // Zero-coupon typically has no periodic payments
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10m,
                DiscountFactor = 1m // No additional discount for simplicity
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
            decimal expectedPresentValue = 1628.89m;

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
                FaceValue = 1000m,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10m,
                DiscountFactor = 0.9m // 10% discount
            };

            // Expected: (1.05)^10 × 1000 × 0.9 ≈ 1628.89 × 0.9 = 1466.01
            decimal expectedPresentValue = 1466.01m;

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
                FaceValue = 0m,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10m,
                DiscountFactor = 1m
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
            var bond = new ZeroCouponBond
            {
                BondId = "ZERO000",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.00%"), 
                FaceValue = 1000m,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10m,
                DiscountFactor = 0m
            };

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0m, result);
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
                FaceValue = 1000m,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 0m,
                DiscountFactor = 0.95m
            };

            // Expected: (1.05)^0 × 1000 × 0.95 = 1 × 1000 × 0.95 = 950.00
            decimal expectedPresentValue = 950.00m;

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
                FaceValue = 1000m,
                PaymentFrequency = "none",
                Rating = "AAA",
                Type = "Government",
                YearsToMaturity = 10m,
                DiscountFactor = 0.9m
            };

            // Expected: (1 + 0)^10 × 1000 × 0.9 = 1 × 1000 × 0.9 = 900.00
            decimal expectedPresentValue = 900m;

            // Act
            var result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expectedPresentValue, result);
        }
    }
}