using Newtonsoft.Json;

namespace AdaptiveBpm.ML.DataCollection;

public static class ReadDataDirectories
{
    private static readonly string DirectoryPath = Directory.GetCurrentDirectory();
    private static readonly string JsonFilePath = Path.Combine(DirectoryPath,"../../../datadirectories.json");

    public static readonly DataDirectory DataDirectory = LoadDataDirectories();

    public static readonly string MasterDataDirectory = DataDirectory.MasterData;
    public static readonly string MasterModelDirectory = DataDirectory.ModelData;
    public static readonly string NewDataDirectory = DataDirectory.NewData;
    public static readonly string UnityDirectory = DataDirectory.UnityDirectory;
    
    private static DataDirectory LoadDataDirectories()
    {
        try
        {
            if (File.Exists(JsonFilePath))
            {
                return JsonConvert.DeserializeObject<DataDirectory>(File.ReadAllText(JsonFilePath));
            }
            else
            {
                // Handle the case when the file does not exist
                Console.WriteLine($"Error: File not found - {DirectoryPath}");
                return null;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., file not accessible
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
            return null;
        }
    }
}

public class DataDirectory
{
    public string MasterData { get; set; }
    public string ModelData { get; set; }
    public string NewData { get; set; }
    public string UnityDirectory { get; set; }
}