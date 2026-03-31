using System.Collections.Generic;
using AdaptiveBpmML;
using AdaptiveBpmML.Models;
using UnityEngine;

namespace AdaptiveBpm
{
    public class MLProcessor
    {
        private const float MinModelIntensity = 1f;
        private const float MaxModelIntensity = 5f;

        private readonly ModelCreation _modelCreation;
        private readonly ModelPrediction _modelPrediction;
        private readonly ModelTraining _modelTraining;
        private readonly bool canWriteToFile;

        public MLProcessor(bool writeToFile)
        {
            canWriteToFile = writeToFile;
            _modelCreation = new ModelCreation();
            _modelPrediction = new ModelPrediction();
            _modelTraining = new ModelTraining();
        }

        public void InitializeModel()
        {
            try
            {
                FileExtensions.EnsurePersistentModelAssets();
                _modelTraining.BuildAndSaveModel();
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Unable to build BPM intensity model. Falling back to heuristic intensity. {exception.Message}");
            }
        }

        public float PredictIntensity(float bpm, float currentIntensity, float targetBpm)
        {
            try
            {
                var sampleInput = new ModelInput
                {
                    Intensity = DenormalizeIntensity(currentIntensity),
                    BPM = bpm,
                    TargetBPM = targetBpm,
                };

                var predictedIntensity = _modelPrediction.PredictIntensity(sampleInput);
                return NormalizeIntensity(predictedIntensity);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Unable to predict intensity from BPM. Using fallback intensity. {exception.Message}");
                return currentIntensity;
            }
        }

        public void AppendTrainingSample(float intensity, float bpm, float targetBpm)
        {
            if (!canWriteToFile)
            {
                return;
            }

            var sampleData = new List<ModelSerialized>
            {
                new()
                {
                    Intensity = DenormalizeIntensity(intensity),
                    BPM = bpm,
                    TargetBPM = targetBpm,
                    BPMDifference = targetBpm - bpm,
                    Label = bpm <= targetBpm ? 1 : 0
                }
            };

            _modelCreation.AppendDataToCSV(sampleData);
        }

        public void RebuildModel()
        {
            if (!canWriteToFile)
            {
                return;
            }

            try
            {
                _modelTraining.BuildAndSaveModel();
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Unable to rebuild BPM intensity model. Continuing with the last saved model. {exception.Message}");
            }
        }

        private static float NormalizeIntensity(float rawIntensity)
        {
            return Mathf.Clamp01(Mathf.InverseLerp(MinModelIntensity, MaxModelIntensity, rawIntensity));
        }

        private static float DenormalizeIntensity(float normalizedIntensity)
        {
            return Mathf.Lerp(MinModelIntensity, MaxModelIntensity, Mathf.Clamp01(normalizedIntensity));
        }
    }
}
