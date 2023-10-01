using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
  public float Ammo;
  public float speed = 0.1f;
  public float InitialAmmo;
  public Text AmmoText;

  private void Start()
  {
    Ammo = InitialAmmo;
  }

  private void Update()
  {
    float horizontal = Input.GetAxis("Horizontal") * speed;
    float vertical = Input.GetAxis("Vertical") * speed;

    transform.Translate(new Vector3(horizontal, 0, vertical));

    if (Input.GetKeyDown(KeyCode.F))
    {
      DecrementAmmo();
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      Jump();
    }

    Debug.Log("Ammo: " + Ammo);
  }

  public void RefreshHPAmmo()
  {
    Ammo = InitialAmmo;
  }

  public void DecrementAmmo()
  {
    Ammo = Mathf.Max(0, Ammo - 1);
  }

  public void Jump()
  {
    GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
  }
}
