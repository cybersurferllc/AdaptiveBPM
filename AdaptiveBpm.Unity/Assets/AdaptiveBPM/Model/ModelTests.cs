using System.Collections.Generic;
using AdaptiveBpmML;
using AdaptiveBpmML.Models;

namespace AdaptiveBpm
{
    public class ModelTests
    {
        private ModelCreation modelCreation;
        private ModelPrediction prediction;
        public ModelTraining training;
        
        public ModelTests()
        {
            modelCreation = new ModelCreation();
            prediction = new ModelPrediction();
            training = new ModelTraining();
        }
        
        public void AddTestData()
        {
            modelCreation.AppendDataToCSV(new List<ModelSerialized>());
        }
        
        public void BuildModel()
        {
            training.BuildAndSaveModel();
        }
        
        public bool TestPrediction(ModelInput input)
        {
            return prediction.PredictBPM(input);
        }
    }
}
