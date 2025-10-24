using CsvHelper;
using CsvHelper.Configuration;
using Domain;
using Domain.Exceptions;
using System;
using System.Globalization;

namespace Infrastructure
{
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

                    var bondData = new BondCreationData
                    {
                        BondID = record.BondID,
                        Issuer = record.Issuer,
                        Rate = record.Rate,
                        FaceValue = record.FaceValue,
                        PaymentFrequency = record.PaymentFrequency,
                        Rating = record.Rating,
                        Type = record.Type,
                        YearsToMaturity = record.YearsToMaturity,
                        DiscountFactor = record.DiscountFactor,
                        DeskNotes = record.DeskNotes
                    };

                    var bond = BondFactory.CreateBond(bondData);
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