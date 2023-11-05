using AdaptiveBpmML;
using Microsoft.ML;

Console.WriteLine("Loading AdaptiveBPM MLModel...");
RunModel();

void RunModel()
{
    AdaptiveBpmMLModel mlModel = new AdaptiveBpmMLModel();
    mlModel.LoadModel();
}

