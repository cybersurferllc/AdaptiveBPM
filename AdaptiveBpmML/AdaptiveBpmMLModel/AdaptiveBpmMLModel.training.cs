using Microsoft.ML;
using Microsoft.ML.Data;

namespace AdaptiveBpmML
{
    public partial class AdaptiveBpmMLTrainingModel
    {
        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: @"Label", featureColumnName: "Features"));
            return pipeline;
        }

        public static IEstimator<ITransformer> BuildPipelineWithLogisticRegression(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: @"Label"));

            return pipeline;
        }

        public static IEstimator<ITransformer> BuildPipelineWithSVM(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.BinaryClassification.Trainers.LinearSvm(labelColumnName: @"Label", numberOfIterations: 100));

            return pipeline;
        }

        public static IEstimator<ITransformer> BuildPipelineWithRandomForest(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.BinaryClassification.Trainers.FastForest(labelColumnName: @"Label", numberOfLeaves: 10, numberOfTrees: 100));
            return pipeline;
        }

        public static IEstimator<ITransformer> BuildPipelineWithStochDualAscent(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: @"Label"));
            return pipeline;
        }

        public static IEstimator<ITransformer> BuildPipelineWithOneVersusAll(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator: mlContext
                    .BinaryClassification.Trainers
                    .FastTree(labelColumnName: "Label", numberOfLeaves: 4, numberOfTrees: 4)));
            return pipeline;
        }

        public static CalibratedBinaryClassificationMetrics EvaluateModel(MLContext mlContext, ITransformer model, IDataView validationData)
        {
            IDataView predictions = model.Transform(validationData);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, @"Label");
            return metrics;
        }
    }
}
