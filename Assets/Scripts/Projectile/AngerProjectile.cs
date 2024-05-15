using System.Collections.Generic;
using UnityEngine;

public class AngerProjectile : Projectile
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
        // the actual callback function is named "onFinish". The animation event function would be "Callback", see anger projectile impact animation.
        callback.AddCallback("onFinish", () => Destroy(this.gameObject));
        // alternative (added functionality to AnimatorCallback)
        // - callback.AddDestroyCallback("onFinish", this.gameObject);
        // this would also work as an example:
        // - callback.AddCallback("awake", Awake);
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
        if (other.tag == Tag.Enemy.ToString() || other.tag == Tag.EnemyBase.ToString()
            || other.tag == Tag.Fence.ToString())
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
        else if (other.tag == Tag.Animal.ToString()
            && other.gameObject.GetComponent<Animal>().GetEmotion() != Emotion.LOVE
            && other.gameObject.GetComponent<Animal>().GetEmotion() != Emotion.ANGER)
            //Disabling single dispatch for now. It's frustrating.
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
        else if(collision.CompareTag("Wall")) // Try only poofing on walls
        {
            //Play same anim but smaller
            GetComponent<Rigidbody>().velocity = Vector3.zero;
                anim.gameObject.transform.localScale = anim.gameObject.transform.localScale / 2;
                anim.SetTrigger("Impact");
        }
        else {
            Physics.IgnoreCollision(collision, GetComponent<Collider>());
        }


        
    }

    protected override void reachMaxDist()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        anim.gameObject.transform.localScale = anim.gameObject.transform.localScale / 2;
        anim.SetTrigger("Impact");
    }
}
