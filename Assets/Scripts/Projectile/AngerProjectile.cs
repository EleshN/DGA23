using System.Collections.Generic;
using UnityEngine;

public class AngerProjectile : Projectile
{

    [SerializeField] Animator anim;
    
    protected override void Awake()
    {
        base.Awake();
        Physics.IgnoreCollision(GetComponent<Collider>(), GameManager.Instance.PlayerObject.GetComponent<Collider>());
    }

    protected override void HandleCollision(Collider collision)
    {
        if (!impacted){
            GameObject other = collision.gameObject;
            if (other.tag == "Enemy")
            {
                HashSet<Animal> animals = GameManager.Instance.followers;
                HashSet<Animal> parrots = new HashSet<Animal>();
                foreach (Animal follower in animals)
                {
                    if (follower.TryGetComponent<Parrot>(out Parrot parrot))
                    {
                        parrots.Add(parrot);
                    }
                    else
                    {
                        follower.ApplyEmotionEffect(Emotion.ANGER, other.transform);
                    }
                }
                animals.Clear(); // no more animals following player
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                anim.SetTrigger("Impact");
                animals.UnionWith(parrots);
                // GameManager.Instance.followers = parrots;
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
        }
    }
}
