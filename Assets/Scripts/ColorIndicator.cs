using System.Collections;
using UnityEngine;

public class ColorIndicator : MonoBehaviour
{
    public Color damageColor = Color.red; // Default damage color
    private Color originalColor;
    private Material objectMaterial;

    void Start()
    {
        // Get the Renderer component and store the original color
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
            originalColor = objectMaterial.color;
        }
    }

    // Call this method to indicate damage
    public void IndicateDamage()
    {
        if (objectMaterial != null)
        {
            StartCoroutine(ChangeColorTemporarily());
        }
    }

    private IEnumerator ChangeColorTemporarily()
    {
        objectMaterial.color = damageColor; // Change to the damage color
        yield return new WaitForSeconds(0.120f); // Wait for 120 milliseconds
        objectMaterial.color = originalColor; // Revert to the original color
    }
}
