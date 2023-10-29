using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
  // The type of ammo this pickup should affect; set this to "love" in the Unity editor
  public string ammoType;

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Trigger Entered by: " + other.tag);  // Debug statement


    if (other.tag == "Player")  // Check if the colliding object has the "Player" tag
    {
      Player playerScript = other.gameObject.GetComponentInParent<Player>();  // Get the Player script component from the object

      Debug.Log("Player script is null: " + (playerScript == null));
      Debug.Log("ammoType is null: " + (ammoType == null));
      Debug.Log("ammoType is null: " + (ammoType == null));
      // Find the index of "love" ammo in the Player's ammoNames array
      int ammoIndexPickup = System.Array.IndexOf(playerScript.ammoNames, ammoType);

      if (ammoIndexPickup >= 0) // Check if the ammo type exists in the array
      {
        playerScript.ammo[ammoIndexPickup] += 1;  // Increment the ammo count by 1
        Destroy(gameObject);  // Remove the pickup object from the game scene
      }
    }
  }
}
