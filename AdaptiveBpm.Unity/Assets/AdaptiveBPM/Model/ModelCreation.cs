using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AdaptiveBpm;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaptiveBpmML
{
    public class ModelCreation
    {
        public void AppendDataToCSV(List<ModelSerialized> data)
        {
            if (data == null || data.Count == 0)
            {
                return;
            }

            FileExtensions.EnsurePersistentModelAssets();
            var unityDataPath = FileExtensions.UnityPeristentDataPathData;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            var directory = Path.GetDirectoryName(unityDataPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var writer = new StreamWriter(unityDataPath, true);
            using var csv = new CsvWriter(writer, config);
            csv.WriteRecords(data);
        }
    }
}
