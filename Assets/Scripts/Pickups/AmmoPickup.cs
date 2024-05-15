using System.Collections;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
  public int ammoType; // Set this to the specified emotion in the Unity editor

  public float delayBeforeReset = 5f; // Time in seconds before the pickup resets

  private SpriteRenderer spriteRenderer;

  private Animator anim;

  private Collider colliderComponent; // Reference to the collider component

  private AnimatorCallback animCallback;

  public AudioSource audioSource;

  public AudioClip pickupSound;
  private void Start()
  {
    // Find the child GameObject named 'Sprite'
    Transform spriteTransform = transform.Find("Sprite");

    // Ensure the sprite transform and renderer are found
    spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
    anim = spriteTransform.GetComponent<Animator>();
    animCallback = spriteTransform.GetComponent<AnimatorCallback>();
    animCallback.AddCallback(nameof(EnablePickup), EnablePickup);
    anim.SetBool("looted", false);

    // Get the collider component
    colliderComponent = GetComponent<Collider>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == Tag.Player.ToString())
    {
      Player playerScript = other.gameObject.GetComponentInParent<Player>();
      int ammoIndexPickup = ammoType;

      // todo: fix this to disallow increments. should be player model's responsability.
      if (ammoIndexPickup >= 0)
      {
        playerScript.AddOneAmmo(ammoIndexPickup); // Increment the ammo count

        anim.SetBool("looted", true);
        anim.SetBool("regrow", false);

        colliderComponent.enabled = false; 
        StartCoroutine(PlayRegrowAnimation());
      }
    }
  }

  private IEnumerator PlayRegrowAnimation()
  {
    yield return new WaitForSeconds(delayBeforeReset);
    anim.SetBool("regrow", true);
    anim.SetBool("looted", true);
    yield return 0;
  }

  // this is called by animation callback
  private void EnablePickup()
  {
    print("begin drawing idle pickup ver");
    anim.SetBool("looted", false);
    anim.SetBool("regrow", false);
    StartCoroutine(EnableCollision());
  }

  private IEnumerator EnableCollision()
  {
    yield return new WaitForFixedUpdate();
    colliderComponent.enabled = true;
    yield return 0;
  }

}
