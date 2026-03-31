using System;
using System.IO;
using AdaptiveBpm;
using Microsoft.ML;

namespace AdaptiveBpmML.Models
{
    public class ModelTraining
    {
        private MLContext mlContext;
        
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext, Microsoft.ML.Data.IDataView dataView)
        {
            var pipeline = mlContext.Transforms
                .Concatenate("Features", nameof(ModelInput.BPM), nameof(ModelInput.TargetBPM), nameof(ModelInput.BPMDifference))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTree(
                    labelColumnName: nameof(ModelInput.Intensity),
                    featureColumnName: "Features"));
            return pipeline;
        }
        
        public void SaveModel(ITransformer model)
        {
            FileExtensions.EnsurePersistentModelAssets();
            var modelPath = FileExtensions.UnityPeristentModelPath;
            using var stream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.None);
            
            mlContext.Model.Save(model, stream);
            Console.WriteLine("Save Model to " + modelPath);
        }
                
        public (Microsoft.ML.Data.IDataView data, Microsoft.ML.Data.IDataView testData) GetDataViews()
        {
            mlContext = new MLContext();
            FileExtensions.EnsurePersistentModelAssets();
            
            var data = mlContext.Data.ReadFromTextFile<ModelInput>(
                FileExtensions.UnityPeristentDataPathData,
                separatorChar: ',',
                hasHeader: false);
            var testData = mlContext.Data.ReadFromTextFile<ModelInput>(
                FileExtensions.UnityPeristentDataPathTestData,
                separatorChar: ',',
                hasHeader: false);

            return (data, testData);
        }

        public ITransformer BuildAndSaveModel()
        {
            var dataViews = GetDataViews();
            var pipeline = BuildPipeline(mlContext, dataViews.data);
            var model = pipeline.Fit(dataViews.data);
            SaveModel(model);
            return model;
        }
    }
}
