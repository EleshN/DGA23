using System.Collections.Generic;
using UnityEngine;

public class LoveProjectile : Projectile
{
    [SerializeField]
    Animator anim;

    protected override void HandleCollision(Collider collision)
    {
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
        //HandleCollision(collision);
    }

}
