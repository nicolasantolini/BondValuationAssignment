using Domain.Exceptions;
using System.Globalization;

namespace Domain
{
    public abstract class Bond
    {
        private decimal _faceValue;
        private decimal _yearsToMaturity;
        private decimal _discountFactor;
        private string _paymentFrequency = "";
        private Rate _rate;
        private string _bondId = "";
        private string _issuer = "";
        private string _rating = "";
        private string _type = "";

        public required string BondId
        {
            get => _bondId;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidBondDataException("BondId is required.", fieldName: nameof(BondId), invalidValue: value);
                _bondId = value;
            }
        }

        public required string Issuer
        {
            get => _issuer;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidBondDataException("Issuer is required.", BondId, nameof(Issuer), value);
                _issuer = value;
            }
        }

        public required Rate Rate
        {
            get => _rate;
            init => _rate = value;
        }

        public required decimal FaceValue
        {
            get => _faceValue;
            init
            {
                if (value < 0)
                    throw new InvalidBondDataException("Face value cannot be negative.", BondId, nameof(FaceValue), value.ToString(CultureInfo.InvariantCulture));
                _faceValue = value;
            }
        }

        public required string PaymentFrequency
        {
            get => _paymentFrequency;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidBondDataException("Payment frequency is required.", BondId, nameof(PaymentFrequency), value);
                _paymentFrequency = value;
                _ = GetPaymentsPerYear(); //to throw exception if invalid
            }
        }

        public required string Rating
        {
            get => _rating;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidBondDataException("Rating is required.", BondId, nameof(Rating), value);
                _rating = value;
            }
        }

        public required string Type
        {
            get => _type;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidBondDataException("Type is required.", BondId, nameof(Type), value);
                _type = value;
            }
        }

        public required decimal YearsToMaturity
        {
            get => _yearsToMaturity;
            init
            {
                if (value < 0)
                    throw new InvalidBondDataException("Years to maturity cannot be negative.", BondId, nameof(YearsToMaturity), value.ToString(CultureInfo.InvariantCulture));
                _yearsToMaturity = value;
            }
        }

        public required decimal DiscountFactor
        {
            get => _discountFactor;
            init
            {
                if (value < 0)
                    throw new InvalidBondDataException("Discount factor cannot be negative.", BondId, nameof(DiscountFactor), value.ToString(CultureInfo.InvariantCulture));
                _discountFactor = value;
            }
        }

        public string? DeskNotes { get; init; }

        public abstract decimal CalculatePresentValue();

        protected int GetPaymentsPerYear()
        {
            if (string.IsNullOrWhiteSpace(PaymentFrequency))
                return 1;

            return PaymentFrequency.ToLowerInvariant() switch
            {
                "quarterly" => 4,
                "semi-annual" => 2,
                "annual" => 1,
                "none" => 0,
                _ => throw new InvalidBondDataException("Invalid payment frequency.", BondId, nameof(PaymentFrequency), PaymentFrequency)
            };
        }
    }
}