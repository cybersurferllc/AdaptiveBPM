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

            // Model (.zip)
            string modelName = "model.zip";
            string modelPath = "B:\\dev\\bpm\\AdaptiveBPM\\AdaptiveBpmML\\models\\" + modelName;

            // Data (.csv)
            string dataName = "test.csv";
            string dataPath = "B:\\dev\\bpm\\AdaptiveBPM\\AdaptiveBpmML\\data\\" + dataName;

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
