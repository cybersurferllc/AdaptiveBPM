using System;
using System.IO;
using AdaptiveBpm;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace AdaptiveBpmML.Models
{
    public class ModelTraining
    {
        private MLContext mlContext;
        private ITransformer loadedModel;
        
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Concatenate("Features", "Intensity", "BPM", "TargetBPM", "BPMDifference")
                .Append(mlContext.BinaryClassification.Trainers.FastTree(
                    labelColumn: @"Label",
                    featureColumn: "Features"));
            return pipeline;
        }
        
        public void SaveModel(ITransformer model)
        {
            var modelPath = FileExtensions.UnityModelPath;
            using var writer = new StreamWriter(modelPath, true);
            
            mlContext.Model.Save(model, writer.BaseStream);
            Console.WriteLine("Save Model to " + modelPath);
        }
                
        public (IDataView data, IDataView testData) GetDataViews()
        {
            // Construct full paths
            var data = mlContext.Data.ReadFromTextFile<ModelInput>(FileExtensions.UnityDataPath,
                separatorChar: ',');
            var testData =
                mlContext.Data.ReadFromTextFile<ModelInput>(FileExtensions.UnityTestDataPath,
                    separatorChar: ',');

            return (data, testData);
        }

        public void LoadModel()
        {
            mlContext = new MLContext();
            var dataViews = GetDataViews();
        
            var pipeline = BuildPipeline(mlContext);
            loadedModel = pipeline.Fit(dataViews.data);

            var predictions = loadedModel.Transform(dataViews.testData);
            var metrics = mlContext.Regression.Evaluate(data: predictions, label: @"Label");
            Console.WriteLine("Model Prediction Accuracy: " + metrics.L1);

            SaveModel(loadedModel);
        }
    }
}