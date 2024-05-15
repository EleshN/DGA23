using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceProjectile : Projectile
{
    [SerializeField] Animator anim;

    [SerializeField] AnimatorCallback callback;

    protected override void Awake()
    {
        base.Awake();
        Physics.IgnoreCollision(GetComponent<Collider>(), GameManager.Instance.PlayerObject.GetComponent<Collider>());
    }

    // Start is called before the first frame update
    void Start()
    {
        callback.AddCallback("onFinish", () => Destroy(this.gameObject));
    }

    protected override void HandleCollision(Collider collision)
    {
        if (collision.isTrigger)
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Scenery"))
        {
            Physics.IgnoreCollision(collision, GetComponent<Collider>());
            return;
        }

        GameObject other = collision.gameObject;
        if (other.tag == Tag.Animal.ToString())
        {
            Animal animal = other.GetComponent<Animal>();
            if (animal.ApplyEmotionEffect(Emotion.DEFENCE))
            {
                GameManager.Instance.followers.Remove(animal);
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("Impact");
        }
        else
        {
            //Play same anim but smaller
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.gameObject.transform.localScale = anim.gameObject.transform.localScale / 2;
            anim.SetTrigger("Impact");
        }
    }

    protected override void reachMaxDist()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        anim.gameObject.transform.localScale = anim.gameObject.transform.localScale / 2;
        anim.SetTrigger("Impact");
    }
}
