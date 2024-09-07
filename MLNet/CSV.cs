using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
namespace MLNet;

public class CSV
{
    private static readonly string filePath = "data/";
    private static readonly string filesToSearch = "data-";
    private static readonly string csvSearchPattern = "*.csv";
    private static readonly string utcFormat = "MM.dd.yyyy_HH.mm.ss";
    private static Func<string, string> masterDataPath = (fileName) => $"masterData-{fileName}.csv";
    
    // read csv files from local path
    public static List<T>? ReadCSVFiles<T>(string path = "")
    {
        var fileSearchPath = $"{filePath}{path}";
        var csvFiles = Directory.EnumerateFiles(path:  fileSearchPath, searchPattern: csvSearchPattern)
            .Where(e => e.Contains(filesToSearch));
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        
        List<T> records = new List<T>();
        foreach (var csvFile in csvFiles){
            string contents = File.ReadAllText(csvFile);
            using var reader = new StringReader(contents);
            
            using var csv = new CsvReader(reader, config);
            var csvRecords = csv.GetRecords<T>();
            records.AddRange(csvRecords);
        }
        return records;
    }
    
    public static void AddDataToModel<T>(List<T> records)
    {
        var directoryPath = filePath;
        var utcNow = DateTime.Now.ToUniversalTime().ToString(utcFormat, CultureInfo.InvariantCulture);
        var csvFile = Path.Combine(directoryPath, masterDataPath(utcNow));

        // Ensure that the directory exists
        Directory.CreateDirectory(directoryPath);

        using var writer = new StreamWriter(csvFile, append: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(records);
    }
}