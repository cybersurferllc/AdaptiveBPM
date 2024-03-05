using AdaptiveBpm.ML.DataCollection;
using AdaptiveBpmML;

Console.WriteLine("Loading AdaptiveBPM MLModel...");
RunModel();

void RunModel()
{
    AdaptiveBpmMLModel mlModel = new AdaptiveBpmMLModel();
    mlModel.LoadModel();
    mlModel.Predict();
}

void WriteNewDataToModel()
{
    string folderPath = ReadDataDirectories.NewDataDirectory;
    
    var records = CsvDataCollection.ReadCSVFiles<BPMData>(folderPath);
    
    CsvDataCollection.AddDataToModel(records);
}
