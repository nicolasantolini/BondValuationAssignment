using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Domain;
using Domain.Exceptions;
using System.Globalization;

namespace Infrastructure
{
    public class FlexibleDecimalConverter : DecimalConverter
    {
        //Helper class to handle both comma and period as decimal separators in the CSV files (given DiscountFactor case)
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return base.ConvertFromString(text, row, memberMapData);
            }

            var style = memberMapData.TypeConverterOptions.NumberStyles ?? NumberStyles.Float | NumberStyles.AllowThousands;
            
            // If the number contains a comma, try to parse it using a culture that uses a comma as a decimal separator.
            if (text.Contains(','))
            {
                var cultureWithComma = new CultureInfo("fr-FR"); // fr-FR uses ',' as a decimal separator.
                if (decimal.TryParse(text, style, cultureWithComma, out var result))
                {
                    return result;
                }
            }

            // Otherwise, or if the above failed, try to parse using the invariant culture (which expects a '.' decimal separator).
            var culture = memberMapData.TypeConverterOptions.CultureInfo ?? CultureInfo.InvariantCulture;
            if (decimal.TryParse(text, style, culture, out var resultInvariant))
            {
                return resultInvariant;
            }

            // Fallback to the base converter to handle potential exceptions.
            return base.ConvertFromString(text, row, memberMapData);
        }
    }

    public class CsvBondParser : IBondParser
    {
        public BondParsingResult ParseBonds(string filePath)
        {
            var result = new BondParsingResult();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            csv.Context.TypeConverterCache.AddConverter<decimal>(new FlexibleDecimalConverter());

            try
            {
                csv.Read();
                csv.ReadHeader();
                csv.ValidateHeader<BondCreationData>();
            }
            catch (HeaderValidationException ex)
            {
                throw new InvalidDataException("The CSV file has invalid headers. Please check the file structure.", ex);
            }


            while (csv.Read())
            {
                BondCreationData? record = null;
                try
                {
                    record = csv.GetRecord<BondCreationData>();

                    if (record is null)
                    {
                        throw new InvalidOperationException("Failed to read CSV record.");
                    }

                    var bond = BondFactory.CreateBond(record);
                    result.Bonds.Add(bond);
                }
                catch (InvalidBondDataException ex)
                {
                    var bondId = record?.BondID ?? "N/A";
                    var rowNumber = csv.Parser.Row;
                    result.Errors.Add($"Invalid data in row {rowNumber} for Bond '{bondId}': {ex.Message}");
                }
                catch (Exception ex)
                {
                    var bondId = record?.BondID ?? "N/A";
                    var rowNumber = csv.Parser.Row;
                    result.Errors.Add($"Error parsing row {rowNumber} for Bond '{bondId}': {ex.Message}");
                }
            }

            return result;
        }
    }

}