using Domain.Exceptions;
using System.Globalization;

namespace Domain
{
    public record Rate
    {
        public double Value { get; init; }

        public static Rate Parse(string rateString, string? bondId = null)
        {
            if (string.IsNullOrWhiteSpace(rateString))
            {
                return new Rate { Value = 0.0};
            }

            rateString = rateString.Trim().TrimEnd('%');

            if (double.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return new Rate { Value = value / 100.0};
            }

            throw new InvalidBondDataException("Invalid rate format.", bondId, nameof(Rate), rateString);
        }
    }
}