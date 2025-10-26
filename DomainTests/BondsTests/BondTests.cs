using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                FaceValue = 1000.0,
                PaymentFrequency = "annual",
                Rating = "AAA",
                Type = "Coupon",
                YearsToMaturity = 10.0,
                DiscountFactor = 0.95
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
            Assert.Equal(0.057, bond.Rate.Value);
            Assert.Equal(1000.0, bond.FaceValue);
            Assert.Equal("annual", bond.PaymentFrequency);
            Assert.Equal("AAA", bond.Rating);
            Assert.Equal("Coupon", bond.Type);
            Assert.Equal(10.0, bond.YearsToMaturity);
            Assert.Equal(0.95, bond.DiscountFactor);
        }
    }

    public class TestBond : Bond
    {
        public override double CalculatePresentValue()
        {
            // Simple implementation for testing base class functionality
            return FaceValue * DiscountFactor;
        }
    }
    
}
