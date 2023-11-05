using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaptiveBpmML
{
    public class AdaptiveBpmMLModel
    {
        private static string MLNetDataPath = Path.GetFullPath("..\\..\\AdaptiveBPM\\AdaptiveBpmML\\data\\test.csv");

        public bool PredictBPM(AdaptiveBpmMLTrainingModel.ModelInput input)
        {
            input.BPMDifference = input.TargetBPM - input.BPM;

            var prediction = AdaptiveBpmMLTrainingModel.Predict(input);

            return prediction.Prediction;
        }

        public void AppendDataToCSV(List<AdaptiveBpmMLTrainingModel.ModelInput> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using (var writer = new StreamWriter(MLNetDataPath, true))
                using (var csv = new CsvWriter(writer, config)){
                    for (int i = 0; i < 10; i++){
                        csv.WriteRecords(data);
                    }
                }
        }
    }
}
