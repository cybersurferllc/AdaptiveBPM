using System;
using AdaptiveBpmML;
using UnityEngine;

namespace AdaptiveBpm
{
    public class TestDataModel : MonoBehaviour
    {
        public ModelTests modelTests;

        public void Start()
        {
            modelTests = new ModelTests();
            // var prediction = modelTests.TestPrediction(new ModelInput(){Intensity = 1, BPM = 150, TargetBPM = 160, BPMDifference = 10, Label = true});
            // Debug.Log(prediction);
            
            modelTests.BuildModel();
        }
    }
}