using Domain;
using Domain.Exceptions;

namespace DomainTests.BondsTests
{
    public class BondValidationTests
    {
        private TestBond CreateValidBond()
        {
            return new TestBond
            {
                BondId = "BOND001",
                Issuer = "US Treasury",
                Rate = Rate.Parse("5.70%"),
                FaceValue = 1000m,
                PaymentFrequency = "annual",
                Rating = "AAA",
                Type = "Coupon",
                YearsToMaturity = 10m,
                DiscountFactor = 0.95m
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void BondId_WhenInvalid_ThrowsException(string invalidBondId)
        {
            // Arrange 
            var bond = CreateValidBond();
            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = invalidBondId,
                    Issuer = bond.Issuer,
                    Rate = bond.Rate,
                    FaceValue = bond.FaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = bond.Rating,
                    Type = bond.Type,
                    YearsToMaturity = bond.YearsToMaturity,
                    DiscountFactor = bond.DiscountFactor
                };
            });

            Assert.Equal(nameof(Bond.BondId), exception.FieldName);
            Assert.Equal(invalidBondId, exception.InvalidValue);
        }

        [Theory]
        [InlineData(-1000)]
        [InlineData(-0.01)]
        public void FaceValue_WhenNegative_ThrowsException(decimal invalidFaceValue)
        {
            // Arrange
            var bond = CreateValidBond();

            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = bond.BondId,
                    Issuer = bond.Issuer,
                    Rate = bond.Rate,
                    FaceValue = invalidFaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = bond.Rating,
                    Type = bond.Type,
                    YearsToMaturity = bond.YearsToMaturity,
                    DiscountFactor = bond.DiscountFactor
                };
            });

            Assert.Equal(nameof(Bond.FaceValue), exception.FieldName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.5)]
        public void YearsToMaturity_WhenNegative_ThrowsException(decimal invalidYears)
        {
            // Arrange
            var bond = CreateValidBond();

            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = bond.BondId,
                    Issuer = bond.Issuer,
                    Rate = bond.Rate,
                    FaceValue = bond.FaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = bond.Rating,
                    Type = bond.Type,
                    YearsToMaturity = invalidYears,
                    DiscountFactor = bond.DiscountFactor
                };
            });

            Assert.Equal(nameof(Bond.YearsToMaturity), exception.FieldName);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        [InlineData(2.0)]
        public void DiscountFactor_WhenInvalid_ThrowsException(decimal invalidFactor)
        {
            // Arrange
            var bond = CreateValidBond();

            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = bond.BondId,
                    Issuer = bond.Issuer,
                    Rate = bond.Rate,
                    FaceValue = bond.FaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = bond.Rating,
                    Type = bond.Type,
                    YearsToMaturity = bond.YearsToMaturity,
                    DiscountFactor = invalidFactor
                };
            });

            Assert.Equal(nameof(Bond.DiscountFactor), exception.FieldName);
        }

        //Missing Issuer throws InvalidBondDataException test
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Issuer_WhenInvalid_ThrowsException(string invalidIssuer)
        {
            // Arrange 
            var bond = CreateValidBond();
            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = bond.BondId,
                    Issuer = invalidIssuer,
                    Rate = bond.Rate,
                    FaceValue = bond.FaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = bond.Rating,
                    Type = bond.Type,
                    YearsToMaturity = bond.YearsToMaturity,
                    DiscountFactor = bond.DiscountFactor
                };
            });
            Assert.Equal(nameof(Bond.Issuer), exception.FieldName);
            Assert.Equal(invalidIssuer, exception.InvalidValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Rating_WhenInvalid_ThrowsException(string invalidRating)
        {
            // Arrange 
            var bond = CreateValidBond();
            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = bond.BondId,
                    Issuer = bond.Issuer,
                    Rate = bond.Rate,
                    FaceValue = bond.FaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = invalidRating,
                    Type = bond.Type,
                    YearsToMaturity = bond.YearsToMaturity,
                    DiscountFactor = bond.DiscountFactor
                };
            });
            Assert.Equal(nameof(Bond.Rating), exception.FieldName);
            Assert.Equal(invalidRating, exception.InvalidValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Type_WhenInvalid_ThrowsException(string invalidType)
        {
            // Arrange 
            var bond = CreateValidBond();
            // Act & Assert
            var exception = Assert.Throws<InvalidBondDataException>(() =>
            {
                var invalidBond = new TestBond
                {
                    BondId = bond.BondId,
                    Issuer = bond.Issuer,
                    Rate = bond.Rate,
                    FaceValue = bond.FaceValue,
                    PaymentFrequency = bond.PaymentFrequency,
                    Rating = bond.Rating,
                    Type = invalidType,
                    YearsToMaturity = bond.YearsToMaturity,
                    DiscountFactor = bond.DiscountFactor
                };
            });
            Assert.Equal(nameof(Bond.Type), exception.FieldName);
            Assert.Equal(invalidType, exception.InvalidValue);
        }

    }

}
