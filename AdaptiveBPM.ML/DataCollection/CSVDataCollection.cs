using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaptiveBpm.ML.DataCollection;

public class CsvDataCollection
{
    // read csv files from local path
    public static List<T> ReadCSVFiles<T>(string folderPath)
    {
        var csvFiles = Directory.EnumerateFiles(path: folderPath, searchPattern: "*.csv").Where(e => e.Contains("data-"));
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        
        List<T> records = new List<T>();
        foreach (var csvFile in csvFiles){
            string contents = File.ReadAllText(csvFile);
            using var reader = new StringReader(contents);
            
            using var csv = new CsvReader(reader, config);
            var newRecords = csv.GetRecords<T>().ToList();
            records.AddRange(newRecords);
        }
        return records;
    }
    
    public static void AddDataToModel<T>(List<T> records)
    {
        var now = DateTime.Now.ToUniversalTime().ToString("MM.dd.yyyy_HH.mm.ss", CultureInfo.InvariantCulture);
        var directoryPath = ReadDataDirectories.MasterDataDirectory;
        var csvModelFile = Path.Combine(directoryPath, $"masterData-{now}.csv");

        // Ensure that the directory exists
        Directory.CreateDirectory(directoryPath);

        using var writer = new StreamWriter(csvModelFile, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(records);
    }
}