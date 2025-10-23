using Xunit;
using Domain;

namespace Domain.Tests
{
    public class CouponBondTests
    {
        [Fact]
        public void CalculatePresentValue_WithRegularRate_ReturnsCorrectValue()
        {
            // Arrange
            var bond = new CouponBond
            {
                BondID = "TEST1",
                Rate = "4.10%",
                FaceValue = 2000m,
                PaymentFrequency = "Semi-Annual",
                YearsToMaturity = 8.8,
                DiscountFactor = 0.67885m
            };

            // Act
            decimal result = bond.CalculatePresentValue();

            // Assert
            // Calculation: 
            // Annual coupon = 4.10% of 2000 = 82
            // Total coupons over 8.8 years = 82 × 8.8 = 721.60
            // Total future value = 2000 + 721.60 = 2721.60
            // Present value = 2721.60 × 0.67885 = 1847.56
            Assert.Equal(1847.56m, result);
        }

        //[Fact]
        //public void CalculatePresentValue_WithInflationLinkedRate_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var bond = new CouponBond
        //    {
        //        BondID = "TEST2",
        //        Rate = "Inflation+0.92%",
        //        FaceValue = 500m,
        //        PaymentFrequency = "Quarterly",
        //        YearsToMaturity = 13.7,
        //        DiscountFactor = 0.54715m
        //    };

        //    // Act
        //    decimal result = bond.CalculatePresentValue();

        //    // Assert
        //    // Calculation:
        //    // Annual coupon = 0.92% of 500 = 4.60
        //    // Total coupons over 13.7 years = 4.60 × 13.7 = 63.02
        //    // Total future value = 500 + 63.02 = 563.02
        //    // Present value = 563.02 × 0.54715 = 308.00
        //    Assert.Equal(308.00m, result);
        //}

        [Theory]
        [InlineData("Quarterly", 4)]
        [InlineData("Semi-Annual", 2)]
        [InlineData("Annual", 1)]
        [InlineData("Unknown", 1)]
        [InlineData("", 1)]
        [InlineData(null, 1)]
        [InlineData("None", 1)]
        public void GetPaymentsPerYear_ReturnsCorrectValue(string frequency, int expected)
        {
            // Arrange
            var bond = new CouponBond { PaymentFrequency = frequency };

            // Act & Assert
            var method = typeof(CouponBond).GetMethod("GetPaymentsPerYear",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = method?.Invoke(bond, null);

            Assert.Equal(expected, result);
        }

        [Theory]
        //[InlineData(1000, "5%", "Annual", 5, 0.8, 1200.00)] // (1000 + 5%×1000×5) × 0.8
        [InlineData(2000, "0%", "Semi-Annual", 10, 0.5, 1000.00)] // (2000 + 0) × 0.5
        [InlineData(500, "10%", "Quarterly", 2, 1.0, 600.00)] // (500 + 10%×500×2) × 1.0
        public void CalculatePresentValue_VariousScenarios_ReturnsExpectedValue(
            decimal faceValue, string rate, string frequency, double years, decimal discount, decimal expected)
        {
            // Arrange
            var bond = new CouponBond
            {
                FaceValue = faceValue,
                Rate = rate,
                PaymentFrequency = frequency,
                YearsToMaturity = years,
                DiscountFactor = discount
            };

            // Act
            decimal result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroFaceValue_ReturnsZero()
        {
            // Arrange
            var bond = new CouponBond
            {
                FaceValue = 0m,
                Rate = "5%",
                PaymentFrequency = "Annual",
                YearsToMaturity = 5,
                DiscountFactor = 0.8m
            };

            // Act
            decimal result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculatePresentValue_WithZeroDiscountFactor_ReturnsZero()
        {
            // Arrange
            var bond = new CouponBond
            {
                FaceValue = 1000m,
                Rate = "5%",
                PaymentFrequency = "Annual",
                YearsToMaturity = 5,
                DiscountFactor = 0m
            };

            // Act
            decimal result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculatePresentValue_WithInvalidRate_ReturnsReasonableValue()
        {
            // Arrange
            var bond = new CouponBond
            {
                FaceValue = 1000m,
                Rate = "InvalidRate",
                PaymentFrequency = "Annual",
                YearsToMaturity = 5,
                DiscountFactor = 0.8m
            };

            // Act
            decimal result = bond.CalculatePresentValue();

            // Assert
            Assert.Equal(800m, result); // Face value × discount factor (rate treated as 0%)
        }
    }
}