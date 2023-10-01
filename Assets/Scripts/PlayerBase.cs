using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Range(0.1f, 10.0f)]
    public float RegenTickSpeed;
    public float HP;
    public GameObject EnemyBaseObject;

    public void DecrementHP(float amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Instantiate(EnemyBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
