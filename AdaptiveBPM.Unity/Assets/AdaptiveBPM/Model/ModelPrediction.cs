using System.IO;
using AdaptiveBpm;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace AdaptiveBpmML
{
    public class ModelPrediction
    {
        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();

            using var stream = new FileStream(FileExtensions.UnityModelPath, FileMode.Open, FileAccess.Read);
            ITransformer mlModel = mlContext.Model.Load(stream);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            return predEngine;
        }
        
        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = CreatePredictEngine();
            return predEngine.Predict(input);
        }

        public float PredictBPM(ModelInput input)
        {
            input.BPMDifference = input.TargetBPM - input.BPM;

            var prediction = Predict(input);

            return prediction.Score;
        }
    }
}
