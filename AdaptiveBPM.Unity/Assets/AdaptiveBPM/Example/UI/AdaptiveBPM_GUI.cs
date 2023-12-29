using System;
using AdaptiveBpm;
using UnityEngine;

public class AdaptiveBPM_GUI : MonoBehaviour
{
    [NonSerialized] private AdaptiveBPM adaptiveBPM;

    // Disable Field Unused warning
#pragma warning disable 0414

    [DebugGUIGraph(min: 50, max: 190, group: 0, r: 1, g: 1f, b: 1f)]
    float currentBPM;

    [DebugGUIGraph(min: 50, max: 190, group: 0, r: .3f, g: 1f, b: 1f)]
    float averageBPM;

    [DebugGUIGraph(min: 0, max: 5, group: 0, r: .5f, g: 1f, b: .5f)]
    float bpmDelta;

    [DebugGUIGraph(min: 0, max: 1, group: 1, r: 1, g: 0.3f, b: 0.3f)]
    float currentIntensity;

    [DebugGUIGraph(min: 0, max: 1, group: 1, r: 1, g: 1f, b: 0.3f)]
    float averageIntensity;

    void Awake()
    {
        adaptiveBPM = FindObjectOfType<AdaptiveBPM>();
    }

    void Update()
    {
        currentBPM = adaptiveBPM.BPM;
        averageBPM = adaptiveBPM.AverageBPM;
        currentIntensity = adaptiveBPM.Intensity;
        bpmDelta = adaptiveBPM.BPMDelta;
        averageIntensity = adaptiveBPM.AverageIntensity;
    }
}
