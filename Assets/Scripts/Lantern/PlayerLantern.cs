using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private MonoBehaviour lightScript;
    [SerializeField] private string intensityPropertyName = "intensity";
    [SerializeField] private float initialDelay = 0.5f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float intensity;
    private PropertyInfo intensityProperty;
    private FieldInfo intensityField;

    private void Start()
    {
        if (lightScript != null)
        {
            System.Type scriptType = lightScript.GetType();
            
            // Get intensity property/field
            intensityProperty = scriptType.GetProperty(intensityPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            intensityField = scriptType.GetField(intensityPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Set initial intensity to 0
            if (intensityProperty != null && intensityProperty.CanWrite)
            {
                intensityProperty.SetValue(lightScript, 0f);
            }
            else if (intensityField != null)
            {
                intensityField.SetValue(lightScript, 0f);
            }
            
            // Start fade in
            StartCoroutine(FadeInLight());
        }
    }

    // Fade light intensity from 0 to 1 after initial delay
    private IEnumerator FadeInLight()
    {
        // Wait for initial delay while keeping intensity at 0
        yield return new WaitForSeconds(initialDelay);
        
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            
            float newIntensity = Mathf.Lerp(0f, intensity, smoothT);
            
            if (intensityProperty != null && intensityProperty.CanWrite)
            {
                intensityProperty.SetValue(lightScript, newIntensity);
            }
            else if (intensityField != null)
            {
                intensityField.SetValue(lightScript, newIntensity);
            }

            yield return null;
        }

        // Ensure final value is set to the desired intensity
        if (intensityProperty != null && intensityProperty.CanWrite)
        {
            intensityProperty.SetValue(lightScript, intensity);
        }
        else if (intensityField != null)
        {
            intensityField.SetValue(lightScript, intensity);
        }
    }
}
