using Domain.Exceptions;
using System.Globalization;

namespace Domain
{
    public abstract class Bond
    {
        private decimal _faceValue;
        private double _yearsToMaturity;
        private decimal _discountFactor;

        public string BondID { get; set; }
        public string Issuer { get; set; }
        public string Rate { get; set; }

        public decimal FaceValue
        {
            get => _faceValue;
            set
            {
                if (value < 0)
                    throw new InvalidBondDataException("Face value cannot be negative.", BondID, nameof(FaceValue), value.ToString());
                _faceValue = value;
            }
        }

        public string PaymentFrequency { get; set; }
        public string Rating { get; set; }
        public string Type { get; set; }

        public double YearsToMaturity
        {
            get => _yearsToMaturity;
            set
            {
                if (value < 0)
                    throw new InvalidBondDataException("Years to maturity cannot be negative.", BondID, nameof(YearsToMaturity), value.ToString());
                _yearsToMaturity = value;
            }
        }

        public decimal DiscountFactor
        {
            get => _discountFactor;
            set
            {
                if (value < 0)
                    throw new InvalidBondDataException("Discount factor cannot be negative.", BondID, nameof(DiscountFactor), value.ToString());
                _discountFactor = value;
            }
        }

        public string DeskNotes { get; set; }

        public abstract decimal CalculatePresentValue();

        protected decimal ParseRate(string rateString)
        {
            if (string.IsNullOrEmpty(rateString))
                return 0m;

            if (rateString.Contains("Inflation+"))
            {
                string percentagePart = rateString.Replace("Inflation+", "").TrimEnd('%');
                if (decimal.TryParse(percentagePart, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal spread))
                {
                    return spread / 100m;
                }
            }
            else
            {
                string cleanRate = rateString.TrimEnd('%');
                if (decimal.TryParse(cleanRate, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                {
                    return rate / 100m;
                }
            }

            throw new InvalidBondDataException("Invalid rate format.", BondID, nameof(Rate), rateString);
        }

        protected int GetPaymentsPerYear()
        {
            if (string.IsNullOrEmpty(PaymentFrequency))
                return 1;

            return PaymentFrequency.ToLower() switch
            {
                "quarterly" => 4,
                "semi-annual" => 2,
                "annual" => 1,
                "none" => 0,
                _ => throw new InvalidBondDataException("Invalid payment frequency.", BondID, nameof(PaymentFrequency), PaymentFrequency)
            };
        }
    }
}