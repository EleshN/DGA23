using UnityEngine;

public class Enemy : MonoBehaviour
{
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Z))
    {
      GameObject[] playerBases = GameObject.FindGameObjectsWithTag("PlayerBase");
      GameObject closest = null;
      float minDist = Mathf.Infinity;

      foreach (GameObject playerBase in playerBases)
      {
        float dist = Vector3.Distance(playerBase.transform.position, transform.position);
        if (dist < minDist)
        {
          closest = playerBase;
          minDist = dist;
        }
      }

      if (closest != null)
      {
        closest.GetComponent<PlayerBase>().DecrementHP(1);
      }
    }
  }
}
