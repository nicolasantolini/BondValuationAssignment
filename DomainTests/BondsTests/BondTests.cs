using Domain;

namespace DomainTests.BondsTests
{
    //This class is to test basic creation and property assignments of Bond
    public class BondTests
    {
        private TestBond CreateValidBond()
        {
            return new TestBond
            {
                BondId = "BOND001",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.70%"), 
                FaceValue = 1000.0m,
                PaymentFrequency = "annual",
                Rating = "AAA",
                Type = "Coupon",
                YearsToMaturity = 10.0m,
                DiscountFactor = 0.95m
            };
        }

        [Fact]
        public void CreateBond_WithValidData_ShouldSucceed()
        {
            // Arrange & Act
            var bond = CreateValidBond();

            // Assert
            Assert.Equal("BOND001", bond.BondId);
            Assert.Equal("US Treasury", bond.Issuer);
            Assert.Equal(0.057m, bond.Rate.Value);
            Assert.Equal(1000.0m, bond.FaceValue);
            Assert.Equal("annual", bond.PaymentFrequency);
            Assert.Equal("AAA", bond.Rating);
            Assert.Equal("Coupon", bond.Type);
            Assert.Equal(10.0m, bond.YearsToMaturity);
            Assert.Equal(0.95m, bond.DiscountFactor);
        }
    }

    public class TestBond : Bond
    {
        public override decimal CalculatePresentValue()
        {
            // Simple implementation for testing base class functionality
            return FaceValue * DiscountFactor;
        }
    }
    
}
