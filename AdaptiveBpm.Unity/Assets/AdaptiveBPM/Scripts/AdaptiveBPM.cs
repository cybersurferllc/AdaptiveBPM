using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptiveBpm
{
    public enum GeneratorType
    {
        Adaptive,
        Fake
    }

    public class AdaptiveBPM : MonoBehaviour
    {
        private IBPMGenerator currentGenerator;
        [SerializeField] private GeneratorType generatorType;
        [SerializeField] private AdaptiveBPMGenerator adaptiveBPMGenerator;
        [SerializeField] private FakeBPMGenerator fakeBPMGenerator;

        private MLProcessor mlProcessor;
        [SerializeField] private bool writeToFile;
        [SerializeField] private float targetBPM = 120f;

        private float currentBpm;
        private float averageBPM;
        private float currentIntensity;
        private float averageIntensity;
        private float bpmDelta;

        [SerializeField] private int historyLength = 5;
        [SerializeField] private float maxBPM;
        [SerializeField] private float minBPM;
        private Queue<float> bpmHistory;

        public float interval = 10f;
        private float intervalElapsedTime = 0f;
        private Queue<float> intensityHistory;

        public Action<float> BPMUpdated;
        public Action<float> IntensityUpdated;

        public float BPM => currentBpm;
        public float AverageBPM => averageBPM;
        public float BPMDelta => bpmDelta;
        public float Intensity => currentIntensity;
        public float AverageIntensity => averageIntensity;

        private int _historyLength;
        public int HistoryLength
        {
            get
            {
                return historyLength;
            }
            set
            {
                historyLength = value;
                ClearQueue(bpmHistory);
                ClearQueue(intensityHistory);
            }
        }

        public void UpdateBPMGenerator(GeneratorType generatorType)
        {
            switch (generatorType)
            {
                case GeneratorType.Adaptive:
                    currentGenerator = adaptiveBPMGenerator;
                    break;
                case GeneratorType.Fake:
                    currentGenerator = fakeBPMGenerator;
                    break;
            }

            this.generatorType = generatorType;
            currentGenerator.BPMUpdated = UpdateBPM;
        }

        private void Start()
        {
            mlProcessor = new MLProcessor(writeToFile);
            mlProcessor.InitializeModel();

            _historyLength = historyLength;
            bpmHistory = new Queue<float>(historyLength);
            intensityHistory = new Queue<float>(historyLength);

            UpdateBPMGenerator(generatorType);
        }

        public void UpdateBPM(float bpm)
        {
            if (bpmHistory.Count >= historyLength)
            {
                bpmHistory.Dequeue();
            }

            bpmHistory.Enqueue(bpm);
            currentBpm = bpm;
            BPMUpdated?.Invoke(currentBpm);

            currentIntensity = CalculateIntensity();
            IntensityUpdated?.Invoke(currentIntensity);
        }

        private float CalculateIntensity()
        {
            if (bpmHistory.Count == 0)
            {
                return 0f;
            }

            float totalBpm = 0f;
            foreach (float bpm in bpmHistory)
            {
                totalBpm += bpm;
            }

            averageBPM = totalBpm / bpmHistory.Count;
            bpmDelta = currentBpm - averageBPM;

            var fallbackIntensity = 1.0f - Mathf.InverseLerp(minBPM, maxBPM, averageBPM);
            return mlProcessor.PredictIntensity(currentBpm, fallbackIntensity, targetBPM);
        }

        private void ClearQueue(Queue<float> queue)
        {
            queue?.Clear();
        }

        private void Update()
        {
            if (historyLength != _historyLength)
            {
                ClearQueue(bpmHistory);
                ClearQueue(intensityHistory);
                _historyLength = historyLength;
            }

            intervalElapsedTime += Time.deltaTime;

            if (intervalElapsedTime >= interval)
            {
                float sumIntensity = 0f;
                foreach (var intensityValue in intensityHistory)
                {
                    sumIntensity += intensityValue;
                }

                if (intensityHistory.Count > 0)
                {
                    averageIntensity = sumIntensity / intensityHistory.Count;
                    mlProcessor.AppendTrainingSample(averageIntensity, averageBPM, targetBPM);
                    mlProcessor.RebuildModel();
                }

                intensityHistory.Clear();
                intervalElapsedTime = 0f;
            }

            if (intensityHistory.Count >= historyLength)
            {
                intensityHistory.Dequeue();
            }

            intensityHistory.Enqueue(currentIntensity);
        }
    }
}
