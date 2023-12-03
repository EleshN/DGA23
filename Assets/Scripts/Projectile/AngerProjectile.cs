using System.Collections.Generic;
using UnityEngine;

public class AngerProjectile : Projectile
{
    // temporary testing
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         this.SetDirection(new Vector3(1, 1, 1));
    //     }
    // }
    [SerializeField]
    Animator anim;

    protected override void HandleCollision(Collider collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy")
        {
            HashSet<Animal> animals = GameManager.Instance.followers;
            foreach (Animal follower in animals)
            {
                follower.ApplyEmotionEffect(Emotion.ANGER, other.transform);
            }
            animals.Clear(); // no more animals following player
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("Impact");
        }
        else if (other.tag == "Animal")
        {
            Animal animal = other.GetComponent<Animal>();
            {
                if (animal.ApplyEmotionEffect(Emotion.ANGER))
                {
                    GameManager.Instance.followers.Remove(animal);
                }
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("Impact");
        }
        else {
            //Play same anim but smaller
            if(!collision.isTrigger)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                anim.gameObject.transform.localScale = anim.gameObject.transform.localScale / 2;
                anim.SetTrigger("Impact");
            }
        }
        //base.HandleCollision(collision);
    }
}
