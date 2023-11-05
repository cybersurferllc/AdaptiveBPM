using Microsoft.ML;

namespace AdaptiveBpmML
{
    public class AdaptiveBpmMLModel
    {
        private MLContext mlContext;
        private ITransformer loadedModel;

        public void LoadModel()
        {
            mlContext = new MLContext();

            // Go back three levels to reach the root of the solution
            string upDirectory = "..\\..\\..";

            // Construct full paths
            string modelPath = Path.Combine(upDirectory, "models", "model.zip");
            string dataPath = Path.Combine(upDirectory, "data", "test.csv");

            Console.WriteLine("Full Model Path: " + modelPath);
            Console.WriteLine("Full Data Path: " + dataPath);

            var data = mlContext.Data.LoadFromTextFile<AdaptiveBpmMLTrainingModel.ModelInput>(dataPath, separatorChar: ',');

            var pipeline = AdaptiveBpmMLTrainingModel.BuildPipeline(mlContext);

            loadedModel = pipeline.Fit(data);
            var predictions = loadedModel.Transform(data);
            var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: @"Label");

            mlContext.Model.Save(loadedModel, data.Schema, modelPath);
        }

        public bool Predict()
        {
            var input = new AdaptiveBpmMLTrainingModel.ModelInput
            {
                Intensity = 4,
                BPM = 90,
                TargetBPM = 110F, // Set your desired BPM here
            };

            input.BPMDifference = input.TargetBPM - input.BPM;

            var prediction = AdaptiveBpmMLTrainingModel.Predict(input);

            return prediction.Prediction;
        }
    }

}
