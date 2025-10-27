using Domain;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests
{
    public class BondFactoryTests
    {
        private BondCreationData CreateBondCreationData(string type = "bond")
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
            var data = CreateBondCreationData("bond");

            // Act
            var bond = BondFactory.CreateBond(data);

            // Assert
            Assert.IsType<CouponBond>(bond);
        }

        [Fact]
        public void CreateBond_WithZeroCouponBondType_ReturnsZeroCouponBond()
        {
            // Arrange
            var data = CreateBondCreationData("zero-coupon");

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
            var data = CreateBondCreationData(type);

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
            var data = CreateBondCreationData(type);

            // Act
            var bond = BondFactory.CreateBond(data);

            // Assert
            Assert.IsType<ZeroCouponBond>(bond);
        }

        [Fact]
        public void CreateBond_WithUnknownBondType_ThrowsArgumentException()
        {
            // Arrange
            var data = CreateBondCreationData("unknown-type");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BondFactory.CreateBond(data));
            Assert.Contains("Unknown bond type", exception.Message);
            Assert.Equal("Type", exception.ParamName);
        }

    }
}