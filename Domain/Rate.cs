using Domain.Exceptions;
using System.Globalization;

namespace Domain
{
    public record Rate
    {
        public decimal Value { get; init; }
        public bool AdjustForInflation { get; init; }

        public static Rate Parse(string rateString, string? bondId = null)
        {
            if (string.IsNullOrWhiteSpace(rateString))
            {
                return new Rate { Value = 0m, AdjustForInflation = false };
            }

            bool adjustForInflation = rateString.Contains("Inflation+", StringComparison.OrdinalIgnoreCase);
            var valuePart = rateString;

            if (adjustForInflation)
            {
                valuePart = rateString.Replace("Inflation+", "", StringComparison.OrdinalIgnoreCase);
            }

            valuePart = valuePart.TrimEnd('%').Trim();

            if (decimal.TryParse(valuePart, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return new Rate { Value = value / 100m, AdjustForInflation = adjustForInflation };
            }

            throw new InvalidBondDataException("Invalid rate format.", bondId, nameof(Rate), rateString);
        }
    }
}