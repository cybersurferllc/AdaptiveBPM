using Microsoft.ML;

namespace AdaptiveBpmML
{
    public class AdaptiveBpmMLModel
    {
        private MLContext mlContext;
        private ITransformer loadedModel;
        string mlDirectory = @"..\..\..";
        string unityDirectory = @"..\..\..\..\AdaptiveBpmUnity\Assets\AdaptiveBpm\Model";

        public (IDataView data, IDataView testData) GetDataViews()
        {
            // Construct full paths
            string dataPath = Path.Combine(unityDirectory, "Data", "data.csv");
            string testDataPath = Path.Combine(unityDirectory, "Data", "test.csv");

            var data = mlContext.Data.LoadFromTextFile<AdaptiveBpmMLTrainingModel.ModelInput>(dataPath,
                separatorChar: ',');
            var testData =
                mlContext.Data.LoadFromTextFile<AdaptiveBpmMLTrainingModel.ModelInput>(testDataPath,
                    separatorChar: ',');

            return (data, testData);
        }


        public void LoadModel()
        {
            mlContext = new MLContext();

            var dataViews = GetDataViews();

            var pipeline = AdaptiveBpmMLTrainingModel.BuildPipeline(mlContext);
            loadedModel = pipeline.Fit(dataViews.data);

            var predictions = loadedModel.Transform(dataViews.testData);
            var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: @"Label");
            Console.WriteLine("Model Prediction Accuracy: " + metrics.Accuracy);

            SaveModel(loadedModel, dataViews.data.Schema);
        }

        public void SaveModel(ITransformer model, DataViewSchema schema)
        {
            string modelPath = Path.Combine(unityDirectory, "models", "model.zip");

            mlContext.Model.Save(model, schema, modelPath);
            Console.WriteLine("Save Model to " + modelPath);
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
