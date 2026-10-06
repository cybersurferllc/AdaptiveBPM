using System.IO;
using AdaptiveBpm;
using Microsoft.ML;

namespace AdaptiveBpmML
{
    public class ModelPrediction
    {
        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            FileExtensions.EnsurePersistentModelAssets();

            using (var stream = new FileStream(FileExtensions.UnityPeristentModelPath, FileMode.Open, FileAccess.Read))
            {
                ITransformer mlModel = mlContext.Model.Load(stream);
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
                return predEngine;
            }
        }
        
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = CreatePredictEngine();
            return predEngine.Predict(input);
        }

        public float PredictIntensity(ModelInput input)
        {
            input.BPMDifference = input.TargetBPM - input.BPM;
            return Predict(input).Score;
        }

        public bool PredictBPM(ModelInput input)
        {
            return PredictIntensity(input) >= input.Intensity;
        }
    }
}
