using System.Collections.Generic;
using AdaptiveBpmML;

namespace AdaptiveBpm
{
    public class MLProcessor
    {
        private AdaptiveBpmMLModel model;

        public bool PredictWithSampleInput()
        {
            var sampleInput = new AdaptiveBpmMLTrainingModel.ModelInput
            {
                Intensity = 4,
                BPM = 90,
                TargetBPM = 110F, // Set your desired BPM here
            };


            model = new AdaptiveBpmMLModel();
            return model.PredictBPM(sampleInput);
        }

        public void AddSampleDataToDatasheet()
        {
            // Sample data to add to the dataset
            var sampleData = new List<AdaptiveBpmMLTrainingModel.ModelInput>
            {
                new AdaptiveBpmMLTrainingModel.ModelInput { Intensity = 1, BPM = 99, TargetBPM = 110, BPMDifference = 10, Label = true },
                new AdaptiveBpmMLTrainingModel.ModelInput { Intensity = 2, BPM = 120, TargetBPM = 130, BPMDifference = 10, Label = true },
                new AdaptiveBpmMLTrainingModel.ModelInput { Intensity = 3, BPM = 130, TargetBPM = 120, BPMDifference = -10, Label = false },
                // Add more data as needed
            };

            model = new AdaptiveBpmMLModel();
            model.AppendDataToCSV(sampleData);
        }
    }
}
