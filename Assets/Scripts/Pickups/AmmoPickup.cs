using UnityEngine;
using System.Collections; // Required for Coroutines

public class AmmoPickup : MonoBehaviour
{
  public string ammoType; // Set this to the specified emotion in the Unity editor
  public UnityEngine.Sprite newSprite; // Assign this in the Unity editor
  public UnityEngine.Sprite originalSprite; // Assign this in the Unity editor

  public float delayBeforeReset = 5f; // Time in seconds before the pickup resets

  private SpriteRenderer spriteRenderer;
  private Collider colliderComponent; // Reference to the collider component

  private void Start()
  {
    // Find the child GameObject named 'Sprite'
    Transform spriteTransform = transform.Find("Sprite");

    // Ensure the sprite transform and renderer are found
    if (spriteTransform != null)
    {
      spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
      if (spriteRenderer != null)
      {
        originalSprite = spriteRenderer.sprite; // Store the original sprite
      }
    }

    // Get the collider component
    colliderComponent = GetComponent<Collider>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      Player playerScript = other.gameObject.GetComponentInParent<Player>();
      int ammoIndexPickup = System.Array.IndexOf(playerScript.ammoNames, ammoType);

      if (ammoIndexPickup >= 0)
      {
        playerScript.ammo[ammoIndexPickup] += 1; // Increment the ammo count
        StartCoroutine(PickupCollected());
      }
    }
  }

  private IEnumerator PickupCollected()
  {
    Debug.Log("PickupCollected coroutine started.");

    // Change to the new sprite
    if (spriteRenderer != null && newSprite != null)
    {
      spriteRenderer.sprite = newSprite;
      Debug.Log("Changed to new sprite.");
    }
    else
    {
      Debug.Log("SpriteRenderer or newSprite is null.");
    }

    // Disable the collider to prevent immediate recollection
    if (colliderComponent != null)
    {
      colliderComponent.enabled = false;
    }

    // Wait for the specified delay
    yield return new WaitForSeconds(delayBeforeReset);
    Debug.Log("Delay finished.");

    // Reset to the original sprite and re-enable the collider
    if (spriteRenderer != null && originalSprite != null)
    {
      spriteRenderer.sprite = originalSprite;
      Debug.Log("Changed back to original sprite.");
    }
    else
    {
      Debug.Log("SpriteRenderer or originalSprite is null.");
    }

    if (colliderComponent != null)
    {
      colliderComponent.enabled = true;
    }
    Debug.Log("Collider re-enabled.");
  }

}
