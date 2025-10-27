using Domain.Exceptions;

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
            double rate = (double)Rate.Value;
            int paymentsPerYear = GetPaymentsPerYear();

            if (paymentsPerYear <= 0)
                throw new InvalidBondDataException("Invalid payment frequency for coupon bond.", BondId, nameof(PaymentFrequency), PaymentFrequency);

            // Coupon bonds: PV = ((1 + (Rate / Payments per Year))^{Years to Maturity × Payments per Year} × Face value) × DF
            double couponPerPeriod = rate / paymentsPerYear;
            double numberOfPeriods = (double)YearsToMaturity * paymentsPerYear;

            double baseValue = 1d + couponPerPeriod;
            double futureValueFactor = Math.Pow(baseValue, numberOfPeriods);

            double presentValue = futureValueFactor * (double)FaceValue * (double)DiscountFactor;

            //Preserving accuracy up to 4 decimal places
            decimal finalPresentValue = (decimal)Math.Round(presentValue * 10000) / 10000m;
            return finalPresentValue;
        }

        
    }
}