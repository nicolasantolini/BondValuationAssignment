using System;
using System.Globalization;

namespace Domain
{
    public class CouponBond : Bond
    {
        public override decimal CalculatePresentValue()
        {
            if (FaceValue == 0 || DiscountFactor == 0)
                return 0m;

            return CalculateCouponBondPresentValue();
        }

        private decimal CalculateCouponBondPresentValue()
        {
            decimal rate = ParseRate(Rate);
            int paymentsPerYear = GetPaymentsPerYear();

            // For coupon bonds, a more standard approach is:
            // PV = (Coupon Payment × PVIFA) + (Face Value × PVIF)
            // Where PVIFA is present value interest factor of annuity
            // and PVIF is present value interest factor

            // Simplified approach: Use the provided discount factor directly
            // on the future value of coupon payments + face value
            //
            decimal annualCouponPayment = rate * FaceValue;
            decimal totalCouponPayments = annualCouponPayment * (decimal)YearsToMaturity;
            decimal totalFutureValue = FaceValue + totalCouponPayments;

            // Apply the discount factor to get present value
            decimal presentValue = totalFutureValue * DiscountFactor;

            return Math.Round(presentValue, 2);
        }

        private int GetPaymentsPerYear()
        {
            if (string.IsNullOrEmpty(PaymentFrequency))
                return 1;

            return PaymentFrequency.ToLower() switch
            {
                "quarterly" => 4,
                "semi-annual" => 2,
                "annual" => 1,
                _ => 1
            };
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