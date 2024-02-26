using System.Collections.Generic;
using AdaptiveBpmML;
using AdaptiveBpmML.Models;
using Microsoft.ML;

namespace AdaptiveBpm
{
    public class ModelTests
    {
        private ModelCreation modelCreation;
        private ModelPrediction prediction;
        private ModelTraining training;
        
        public void AddTestData()
        {
            // ml context
            var mlContext = new MLContext();
            
            // add data
            modelCreation.AppendDataToCSV(new List<ModelSerialized>());
            
            // train model
            var dataViews = training.GetDataViews();
            var pipeline = ModelTraining.BuildPipeline(mlContext);
            var model = pipeline.Fit(dataViews.data);
            
            // save model
            training.SaveModel(model);
        }
        
        public float TestPrediction(ModelInput input)
        {
            return prediction.PredictBPM(input);
        }
    }
}