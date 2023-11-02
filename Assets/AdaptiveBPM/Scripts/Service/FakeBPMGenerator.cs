using System;
using UnityEngine;

public class FakeBPMGenerator : MonoBehaviour, IBPMGenerator
{
    private float bpm;
    public Action<float> bpmUpdated;
    public float BPM { get => bpm; }
    public Action<float> BPMUpdated { get => bpmUpdated; set => bpmUpdated = value; }

    [SerializeField] private float updateInterval = 1.0f; // Time in seconds between each BPM update
    [SerializeField] private int maxBPM = 160; // Maximum BPM
    [SerializeField] private int minBPM = 60; // Minimum BPM
    [SerializeField] private float staticBPM = 0f; // The static BPM value. If it's not 0, it overrides the min, max range.
    private float elapsedTime = 0f; // Used to track elapsed time since last BPM update

    private void Update()
    {
        elapsedTime += Time.deltaTime; // Increment the elapsed time counter

        if (elapsedTime >= updateInterval)
        {
            GenerateBPM();
            elapsedTime = 0f; // Reset the counter
        }
    }

    void GenerateBPM()
    {
        int generatedBPM;

        // Check if staticBPM is not 0
        if (Mathf.Abs(staticBPM) > 0.001f)  // We use a small threshold instead of direct comparison due to floating point inaccuracies
        {
            generatedBPM = Mathf.RoundToInt(staticBPM);  // Convert the staticBPM to int
        }
        else
        {
            generatedBPM = UnityEngine.Random.Range(minBPM, maxBPM + 1); // +1 because Random.Range's max is exclusive for ints
        }

        BPMUpdated?.Invoke(generatedBPM);
    }

    public void UpdateInterval(float newInterval)
    {
        updateInterval = newInterval;
    }
}