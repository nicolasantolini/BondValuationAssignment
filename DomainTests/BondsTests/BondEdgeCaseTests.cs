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
                FaceValue = 0.0,
                PaymentFrequency = "semi-annual",
                Rating = "BBB",
                Type = "Zero-Coupon",
                YearsToMaturity = 5.0,
                DiscountFactor = 0.90
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
                FaceValue = 1000.0,
                PaymentFrequency = "quarterly",
                Rating = "AA",
                Type = "Coupon",
                YearsToMaturity = 0.0,
                DiscountFactor = 0.85
            };

            // Assert
            Assert.Equal(0.0, bond.YearsToMaturity);
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
                FaceValue = 1000.0,
                PaymentFrequency = "annual",
                Rating = "A",
                Type = "Coupon",
                YearsToMaturity = 15.0,
                DiscountFactor = 0.0
            };
            var bondOne = new TestBond
            {
                BondId = "BOND005",
                Issuer = "Test Issuer",
                Rate = Rate.Parse("6.00%"),
                FaceValue = 1000.0,
                PaymentFrequency = "annual",
                Rating = "A",
                Type = "Coupon",
                YearsToMaturity = 15.0,
                DiscountFactor = 1.0
            };

            // Assert
            Assert.Equal(0.0, bondZero.DiscountFactor);
            Assert.Equal(1.0, bondOne.DiscountFactor);
        }
    }
}
