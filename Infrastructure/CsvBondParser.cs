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

            // CsvHelper does not load all records at once, so we can process them one by one.
            while (csv.Read())
            {
                CsvBondRecord record = null;
                try
                {
                    record = csv.GetRecord<CsvBondRecord>();

                    var bond = BondFactory.CreateBond(record.Type);

                    // Map properties
                    bond.BondID = record.BondID;
                    bond.Issuer = record.Issuer;
                    bond.Rate = record.Rate;
                    bond.FaceValue = record.FaceValue;
                    bond.PaymentFrequency = record.PaymentFrequency;
                    bond.Rating = record.Rating;
                    bond.Type = record.Type;
                    bond.YearsToMaturity = record.YearsToMaturity;
                    bond.DiscountFactor = record.DiscountFactor;
                    bond.DeskNotes = record.DeskNotes;


                    result.Bonds.Add(bond);
                }
                catch (InvalidBondDataException ex)
                {
                    var bondId = record?.BondID ?? "N/A";
                    var rowNumber = csv.Parser.Row;
                    result.Errors.Add($"Invalid data in row {rowNumber}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    var bondId = record?.BondID ?? "N/A";
                    var rowNumber = csv.Parser.Row;
                    result.Errors.Add($"Error parsing row {rowNumber} for BondID '{bondId}': {ex.Message}");
                }
            }

            return result;
        }
    }

    // Helper class for CSV mapping
    public class CsvBondRecord
    {
        public string BondID { get; set; }
        public string Issuer { get; set; }
        public string Rate { get; set; }
        public decimal FaceValue { get; set; }
        public string PaymentFrequency { get; set; }
        public string Rating { get; set; }
        public string Type { get; set; }
        public double YearsToMaturity { get; set; }
        public decimal DiscountFactor { get; set; }
        public string DeskNotes { get; set; }
    }
}