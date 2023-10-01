using UnityEngine;

public class RegenTrigger : MonoBehaviour
{
  private PlayerBase playerBase;
  private float nextRegenTime = 0.0f;

  private void Start()
  {
    // Assuming the PlayerBase script is attached to the parent GameObject
    playerBase = transform.parent.GetComponent<PlayerBase>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      nextRegenTime = Time.time + playerBase.RegenTickSpeed;
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.tag == "Player" && Time.time >= nextRegenTime)
    {
      other.gameObject.GetComponent<Player>().RefreshHPAmmo();
      nextRegenTime = Time.time + playerBase.RegenTickSpeed;
    }
  }
}
