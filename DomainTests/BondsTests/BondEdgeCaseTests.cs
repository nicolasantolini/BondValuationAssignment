using Domain;

namespace DomainTests.BondsTests
{
    //This class is to test edge cases for Bond creation and properties
    public class BondEdgeCaseTests
    {

        [Fact]
        public void CreateBond_WithZeroFaceValue_ShouldSucceed()
        {
            // Arrange & Act
            var bond = new TestBond
            {
                BondId = "BOND002",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("3.50%"),
                FaceValue = 0m,
                PaymentFrequency = "semi-annual",
                Rating = "BBB",
                Type = "Zero-Coupon",
                YearsToMaturity = 5m,
                DiscountFactor = 0.9m
            };

            // Assert
            Assert.Equal(0, bond.FaceValue);
        }

        [Fact]
        public void CreateBond_WithZeroYearsToMaturity_ShouldSucceed()
        {
            // Arrange & Act
            var bond = new TestBond
            {
                BondId = "BOND003",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("4.25%"),
                FaceValue = 1000m,
                PaymentFrequency = "quarterly",
                Rating = "AA",
                Type = "Coupon",
                YearsToMaturity = 0m,
                DiscountFactor = 0.85m
            };

            // Assert
            Assert.Equal(0m, bond.YearsToMaturity);
        }

        [Fact]
        public void CreateBond_WithBoundaryDiscountFactor_ShouldSucceed()
        {
            // Arrange & Act
            var bondZero = new TestBond
            {
                BondId = "BOND004",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("6.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "A",
                Type = "Coupon",
                YearsToMaturity = 15m,
                DiscountFactor = 0m
            };
            var bondOne = new TestBond
            {
                BondId = "BOND005",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("6.00%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "A",
                Type = "Coupon",
                YearsToMaturity = 15m,
                DiscountFactor = 1m
            };

            // Assert
            Assert.Equal(0m, bondZero.DiscountFactor);
            Assert.Equal(1m, bondOne.DiscountFactor);
        }
    }
}
