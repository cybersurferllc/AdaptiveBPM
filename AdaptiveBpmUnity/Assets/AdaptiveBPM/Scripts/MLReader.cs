using AdaptiveBpmML;

namespace AdaptiveBpm
{
    public class MLReader
    {
        public bool PredictWithSampleInput()
        {
            var sampleInput = new AdaptiveBpmMLTrainingModel.ModelInput
            {
                Intensity = 4,
                BPM = 90,
                TargetBPM = 110F, // Set your desired BPM here
            };


            AdaptiveBpmMLModel model = new AdaptiveBpmMLModel();
            return model.PredictBPM(sampleInput);
        }
    }
}
