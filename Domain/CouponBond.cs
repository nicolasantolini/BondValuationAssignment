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
            decimal rate = Rate.Value;
            int paymentsPerYear = GetPaymentsPerYear();

            // Coupon bonds: PV = ((1 + (Rate / Payments per Year))^{Years to Maturity × Payments per Year} × Face value) × DF
            decimal couponPerPeriod = rate / paymentsPerYear;
            decimal numberOfPeriods = (decimal)(YearsToMaturity * paymentsPerYear);

            double baseValue = 1.0 + (double)couponPerPeriod;
            double exponent = (double)numberOfPeriods;
            double futureValueFactor = Math.Pow(baseValue, exponent);

            decimal presentValue = (decimal)futureValueFactor * FaceValue * DiscountFactor;

            return Math.Round(presentValue, 2);
        }

        
    }
}