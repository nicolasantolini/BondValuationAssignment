using Domain.Exceptions;
using System.Globalization;

namespace Domain
{
    public record Rate
    {
        public decimal Value { get; init; }

        public static Rate Parse(string rateString, string? bondId = null)
        {
            if (string.IsNullOrWhiteSpace(rateString))
            {
                return new Rate { Value = 0m};
            }

            rateString = rateString.Trim().TrimEnd('%');

            if (decimal.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return new Rate { Value = value / 100m};
            }

            throw new InvalidBondDataException("Invalid rate format.", bondId, nameof(Rate), rateString);
        }
    }
}