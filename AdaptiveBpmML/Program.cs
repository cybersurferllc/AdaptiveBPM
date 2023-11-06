using AdaptiveBpmML;

Console.WriteLine("Loading AdaptiveBPM MLModel...");
RunModel();

void RunModel()
{
    AdaptiveBpmMLModel mlModel = new AdaptiveBpmMLModel();
    mlModel.LoadModel();
}
