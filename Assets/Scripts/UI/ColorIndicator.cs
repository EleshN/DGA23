using System.Collections;
using UnityEngine;

public class ColorIndicator : MonoBehaviour
{
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Red with transparency
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Find the child GameObject named 'Sprite'
        Transform spriteTransform = transform.Find("Sprite");

        // Check if the child was found
        if (spriteTransform != null)
        {
            // Get the SpriteRenderer component from the child and assign it to the class member
            spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

            // Check if the SpriteRenderer component is attached
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
            else
            {
                Debug.Log("SpriteRenderer component not found on the child GameObject.");
            }
        }
        else
        {
            Debug.Log("Child GameObject named 'Sprite' not found at " + gameObject.name);
        }
    }


    // Call this method to indicate damage
    public void IndicateDamage()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashColorTemporarily());
        }
    }

    private IEnumerator FlashColorTemporarily()
    {
        float flashDuration = 0.300f; // Duration of the flash
        float flashElapsedTime = 0;

        while (flashElapsedTime < flashDuration)
        {
            flashElapsedTime += Time.deltaTime;
            float lerpFactor = Mathf.Abs(Mathf.Sin((flashElapsedTime / flashDuration) * Mathf.PI)); // Sin wave for smooth interpolation
            spriteRenderer.color = Color.Lerp(originalColor, damageColor, lerpFactor);
            yield return null;
        }

        spriteRenderer.color = originalColor; // Revert to the original color
    }
}
