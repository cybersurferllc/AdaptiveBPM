using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private AdaptiveBPM adaptiveBPM;

    [Header("Movement Settings")]
    public float intensityMultiplier = 5f;  // Controls the extent to which the sphere will move based on intensity.
    public float lerpSpeed = 2f;  // Controls the speed of the lerp. Higher values will make it move to the target faster.

    [Header("Material Emission Settings")]
    public Renderer targetRenderer; // Drag the GameObject's Renderer (like MeshRenderer) here.
    public Color baseEmissionColor = Color.white; // The base color of the emission.

    [Header("Light Intensity Settings")]
    public Light targetLight; // Drag the Light component here.
    public float maxLightIntensity = 2f; // The maximum intensity for the light when Intensity is at its peak.

    private float targetYPosition;  // This will store the target Y position based on intensity.

    private void Start()
    {
        if (targetRenderer && !targetRenderer.material.IsKeywordEnabled("_EMISSION"))
        {
            Debug.LogWarning("Emission keyword is not enabled on the target material. Ensure the material supports emission.");
        }
    }

    private void Update()
    {
        float intensity = adaptiveBPM.Intensity;
        targetYPosition = intensity * intensityMultiplier;

        // Lerp current position to the target position.
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(currentPosition.x, targetYPosition, currentPosition.z);
        transform.position = Vector3.Lerp(currentPosition, targetPosition, lerpSpeed * Time.deltaTime);

        // Update material emission.
        if (targetRenderer && targetRenderer.material.HasProperty("_EmissionColor"))
        {
            Color finalEmissionColor = baseEmissionColor * intensity;
            targetRenderer.material.SetColor("_EmissionColor", finalEmissionColor);
        }

        // Update light intensity.
        if (targetLight)
        {
            targetLight.intensity = intensity * maxLightIntensity;
        }
    }
}