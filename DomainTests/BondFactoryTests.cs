using Domain;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests
{
    public class BondFactoryTests
    {
        private BondCreationData CreateValidBondCreationData(string type = "bond")
        {
            return new BondCreationData
            {
                BondID = "TEST001",
                Issuer = "Test Issuer",
                Rate = "5%",
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AAA",
                Type = type,
                YearsToMaturity = 10m,
                DiscountFactor = 0.95m,
                DeskNotes = "Test notes"
            };
        }

        [Fact]
        public void CreateBond_WithCouponBondType_ReturnsCouponBond()
        {
            // Arrange
            var data = CreateValidBondCreationData("bond");

            // Act
            var bond = BondFactory.CreateBond(data);

            // Assert
            Assert.IsType<CouponBond>(bond);
            Assert.Equal(data.BondID, bond.BondId);
            Assert.Equal(data.Issuer, bond.Issuer);
            Assert.Equal(data.FaceValue, bond.FaceValue);
            Assert.Equal(data.PaymentFrequency, bond.PaymentFrequency);
            Assert.Equal(data.Rating, bond.Rating);
            Assert.Equal(data.Type, bond.Type);
            Assert.Equal(data.YearsToMaturity, bond.YearsToMaturity);
            Assert.Equal(data.DiscountFactor, bond.DiscountFactor);
            Assert.Equal(data.DeskNotes, bond.DeskNotes);
        }

        [Fact]
        public void CreateBond_WithZeroCouponBondType_ReturnsZeroCouponBond()
        {
            // Arrange
            var data = CreateValidBondCreationData("zero-coupon");

            // Act
            var bond = BondFactory.CreateBond(data);

            // Assert
            Assert.IsType<ZeroCouponBond>(bond);
        }

        [Theory]
        [InlineData("BOND")]
        [InlineData("Bond")]
        [InlineData("BOnD")]
        [InlineData(" bond ")]
        public void CreateBond_WithCaseInsensitiveAndWhitespace_CreatesCorrectBondType(string type)
        {
            // Arrange
            var data = CreateValidBondCreationData(type);

            // Act
            var bond = BondFactory.CreateBond(data);

            // Assert
            Assert.IsType<CouponBond>(bond);
        }

        [Theory]
        [InlineData("ZERO-COUPON")]
        [InlineData("Zero-Coupon")]
        [InlineData("zero-coupon ")]
        public void CreateBond_WithZeroCouponVariations_CreatesCorrectBondType(string type)
        {
            // Arrange
            var data = CreateValidBondCreationData(type);

            // Act
            var bond = BondFactory.CreateBond(data);

            // Assert
            Assert.IsType<ZeroCouponBond>(bond);
        }

        [Fact]
        public void CreateBond_WithUnknownBondType_ThrowsArgumentException()
        {
            // Arrange
            var data = CreateValidBondCreationData("unknown-type");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BondFactory.CreateBond(data));
            Assert.Contains("Unknown bond type", exception.Message);
            Assert.Equal("Type", exception.ParamName);
        }

    }
}