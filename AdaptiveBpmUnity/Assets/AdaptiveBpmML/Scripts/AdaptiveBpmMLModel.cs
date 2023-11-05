namespace AdaptiveBpmML
{
    public class AdaptiveBpmMLModel
    {
        public bool PredictBPM(AdaptiveBpmMLTrainingModel.ModelInput input)
        {
            input.BPMDifference = input.TargetBPM - input.BPM;

            var prediction = AdaptiveBpmMLTrainingModel.Predict(input);

            return prediction.Prediction;
        }
    }

}
