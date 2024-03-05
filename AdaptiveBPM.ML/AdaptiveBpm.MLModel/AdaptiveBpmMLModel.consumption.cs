using System.IO;
using AdaptiveBpm.ML.DataCollection;
using Microsoft.ML;
using Microsoft.ML.Data;
namespace AdaptiveBpmML
{
    public partial class AdaptiveBpmMLTrainingModel
    {
        /// <summary>
        /// model input class for AdaptiveBpmMLModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"Intensity")]
            [LoadColumn(0)]
            public float Intensity { get; set; }

            [ColumnName(@"BPM")]
            [LoadColumn(1)]
            public float BPM { get; set; }

            [ColumnName(@"TargetBPM")]
            [LoadColumn(2)]
            public float TargetBPM { get; set; }

            [ColumnName(@"BPMDifference")]
            [LoadColumn(3)]
            public float BPMDifference { get; set; }

            [ColumnName(@"Label")]
            [LoadColumn(4)]
            public bool Label { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for AdaptiveBpmMLModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName("PredictedLabel")]
            public bool Prediction { get; set; }
            [ColumnName("Score")]
            public float Score { get; set; }
        }

        #endregion

        /// <summary>
        /// model serialized class for AdaptiveBpmMLModel.
        /// </summary>
        #region model serialized class
        public class ModelSerialized
        {
            [ColumnName(@"Intensity")]
            public float Intensity { get; set; }

            [ColumnName(@"BPM")]
            public float BPM { get; set; }

            [ColumnName(@"TargetBPM")]
            public float TargetBPM { get; set; }

            [ColumnName(@"BPMDifference")]
            public float BPMDifference { get; set; }

            [ColumnName(@"Label")]
            public int Label { get; set; }
        }

        #endregion

        private static string UnityModelPath =
            Path.Combine(ReadDataDirectories.UnityDirectory, @"models\\model.zip");

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

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();

            using (var stream = new FileStream(UnityModelPath, FileMode.Open, FileAccess.Read))
            {
                ITransformer mlModel = mlContext.Model.Load(stream, out var schema);
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
                return predEngine;
            }
        }
    }
}
