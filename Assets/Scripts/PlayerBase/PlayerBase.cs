using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageable
{
    [Range(0.1f, 10.0f)]
    public float RegenTickSpeed;
    public float HP;
    public GameObject EnemyBaseObject;

    public void Start()
    {
        GameManager.Instance.Register(this);
    }
    public void TakeDamage(float amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            GameManager.Instance.Unregister(this);
            Instantiate(EnemyBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
