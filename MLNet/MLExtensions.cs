using Microsoft.ML;

namespace MLNet;

public static class MLExtensions
{
    public static ITransformer? GetMLContext(ref MLContext context, string path)
    {
        return string.IsNullOrEmpty(path)
            ? null
            : context.Model.Load(path, out var schema);
    }
    
    public static float Predict(this MLContext context, ITransformer model, HeartRateData input)
    {
        var engine = context.Model.CreatePredictionEngine<HeartRateData, HeartRatePrediction>(model);
        var prediction = engine.Predict(input);
        return prediction.Intensity;
    }
}