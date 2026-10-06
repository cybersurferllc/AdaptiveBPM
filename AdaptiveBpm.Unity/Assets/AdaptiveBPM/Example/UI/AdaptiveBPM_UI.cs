using AdaptiveBpm;
using UnityEngine;
using UnityEngine.UI;

public class AdaptiveBPM_UI : MonoBehaviour
{
    [SerializeField] Text BPMText;
    [SerializeField] Text IntensityText;

    public void Start()
    {
        var adaptiveBPM = FindObjectOfType<AdaptiveBPM>();
        if (adaptiveBPM != null)
        {
            adaptiveBPM.BPMUpdated = bpm => UpdateBPM(bpm);
            adaptiveBPM.IntensityUpdated = intensity => UpdateIntensity(intensity);
        }
    }

    public void UpdateBPM(float bpm) => BPMText.text = $"BPM: {bpm}";
    public void UpdateIntensity(float intensity) => IntensityText.text = $"Intensity: {intensity}";
}
