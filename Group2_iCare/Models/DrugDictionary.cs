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
        private readonly List<Drug> drugs;

        public DrugDictionary(string csvFilePath)
        {
            drugs = LoadDrugsFromCsv(csvFilePath);
        }

        private List<Drug> LoadDrugsFromCsv(string csvFilePath)
        {
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Drug>().ToList();
            }
        }

        public IEnumerable<object> GetDrugSuggestions(string query)
        {
            if (drugs == null || string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<object>();
            }

            var suggestions = drugs
                .Where(drug => !string.IsNullOrEmpty(drug.DrugName) &&
                               drug.DrugName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(drug => new
                {
                    drug.DrugName,
                    drug.DrugDescription
                })
                .ToList();

            return suggestions;
        }
    }
}
