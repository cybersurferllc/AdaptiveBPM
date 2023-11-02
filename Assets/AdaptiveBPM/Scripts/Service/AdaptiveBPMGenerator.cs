using System;
using UnityEngine;

[RequireComponent(typeof(hyperateSocket))]
public class AdaptiveBPMGenerator : MonoBehaviour, IBPMGenerator
{
    [SerializeField] private hyperateSocket hyperateSocket;

    private float bpm;
    public Action<float> bpmUpdated;
    public float BPM { get => bpm; }
    public Action<float> BPMUpdated { get => bpmUpdated; set => bpmUpdated = value; }

    private void Start()
    {
        hyperateSocket.BPMUpdated = bpm => BPMUpdated?.Invoke(bpm);
    }
}