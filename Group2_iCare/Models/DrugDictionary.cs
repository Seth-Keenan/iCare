using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace Group2_iCare.Models
{
    public class DrugDictionary
    {
        private readonly List<Drug> drugs; // list of drugs

        public DrugDictionary(string csvFilePath)
        {
            drugs = LoadDrugsFromCsv(csvFilePath); // load drugs from csv file constructor 
        }

        private List<Drug> LoadDrugsFromCsv(string csvFilePath) // method to load drugs from csv file
        {
            using (var reader = new StreamReader(csvFilePath)) // read csv file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Drug>().ToList(); // return the list of drugs
            }
        }

        public IEnumerable<object> GetDrugSuggestions(string query)
        {
            if (drugs == null || string.IsNullOrWhiteSpace(query)) // if drugs is null or query is empty
            {
                return Enumerable.Empty<object>(); // return empty
            }

            var suggestions = drugs // drugs suggestions
                .Where(drug => !string.IsNullOrEmpty(drug.DrugName) &&
                               drug.DrugName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) // string comparison for match
                .Select(drug => new // select drug that matches
                {
                    drug.DrugName,
                    drug.DrugDescription
                })
                .ToList();

            return suggestions; // return suggestions
        }
    }
}