using System;
using System.Globalization;

namespace Domain
{
    public class ZeroCouponBond : Bond
    {
        public override decimal CalculatePresentValue()
        {
            if (FaceValue == 0 || DiscountFactor == 0)
                return 0m;

            // Zero-coupon bonds: PV = ((1 + Rate)^{Years to Maturity} × Face value) × DF
            decimal rate = ParseRate(Rate);
            decimal futureValueFactor = (decimal)Math.Pow((double)(1 + rate), YearsToMaturity);
            decimal presentValue = (futureValueFactor * FaceValue) * DiscountFactor;

            return Math.Round(presentValue, 2);
        }

        private decimal ParseRate(string rateString)
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
                return 0m;
            }
            else
            {
                string cleanRate = rateString.TrimEnd('%');
                if (decimal.TryParse(cleanRate, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                {
                    return rate / 100m;
                }
                return 0m;
            }
        }
    }
}