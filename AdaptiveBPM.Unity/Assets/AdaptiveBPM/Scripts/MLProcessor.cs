using System.Collections.Generic;
using AdaptiveBpmML;

namespace AdaptiveBpm
{
    public class MLProcessor
    {
        private ModelCreation _modelCreation;
        private ModelPrediction _modelPrediction;
        private bool canWriteToFile;

        public MLProcessor(bool writeToFile)
        {
            canWriteToFile = writeToFile;
        }

        public float PredictWithSampleInput()
        {
            var sampleInput = new ModelInput
            {
                Intensity = 4,
                BPM = 90,
                TargetBPM = 110F, // Set your desired BPM here
            };

            _modelPrediction = new ModelPrediction();
            return _modelPrediction.PredictBPM(sampleInput);
        }

        public void AddSampleDataToDatasheet()
        {
            if (!canWriteToFile)
            {
                return;
            }

            // Sample data to add to the dataset
            var sampleData = new List<ModelSerialized>
            {
                new() { Intensity = 1, BPM = 99, TargetBPM = 110, BPMDifference = 10, Label = 1 },
                new() { Intensity = 2, BPM = 120, TargetBPM = 130, BPMDifference = 10, Label = 1 },
                new() { Intensity = 3, BPM = 130, TargetBPM = 120, BPMDifference = -10, Label = 0 },
                // Add more data as needed
            };

            _modelCreation = new ModelCreation();
            _modelCreation.AppendDataToCSV(sampleData);
        }
    }
}
