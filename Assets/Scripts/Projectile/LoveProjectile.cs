using System.Collections.Generic;
using UnityEngine;

public class LoveProjectile : Projectile
{

    [SerializeField] Animator anim;

    [SerializeField] AnimatorCallback callback;

    protected override void Awake()
    {
        base.Awake();
        Physics.IgnoreCollision(GetComponent<Collider>(), GameManager.Instance.PlayerObject.GetComponent<Collider>());
    }

    protected void Start()
    {
        callback.AddDestroyCallback("onFinish", this.gameObject);
    }

    protected override void HandleCollision(Collider collision)
    {
        // ignore hits with triggers (goal of emotional projectiles are to hit solid colliders)
        if (collision.isTrigger)
        {
            return;
        }
        GameObject other = collision.gameObject;
        if (other.tag == "Animal")
        {
            Animal animal = other.GetComponent<Animal>();
            if (animal.ApplyEmotionEffect(Emotion.LOVE, GameManager.Instance.PlayerTransform))
            {
                GameManager.Instance.followers.Add(animal);
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("Impact");
        }
        else {
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
