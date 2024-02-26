using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AdaptiveBpm;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace AdaptiveBpmML
{
    public class ModelCreation
    {
        private MLContext mlContext;
        private ITransformer loadedModel;
        
        public void AppendDataToCSV(List<ModelSerialized> data)
        {
            var unityDirectory = FileExtensions.unityDirectory;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using var writer = new StreamWriter(unityDirectory, true);
            using var csv = new CsvWriter(writer, config);
            for (int i = 0; i < 10; i++){
                csv.WriteRecords(data);
            }
        }
    }
}
