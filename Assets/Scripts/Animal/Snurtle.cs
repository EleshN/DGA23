using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snurtle : Animal
{
    [Header("Snurtle Attack Stats")]
    [SerializeField] Hitbox hitbox;

    [Tooltip("the entities that the snurtle can attak")]
    [SerializeField] Tag[] targets;
    [SerializeField] float attackDelay; //TODO

    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime; //TODO

    [Header("Snurtle SFX")]
    public AudioSource snurtleAudioSource;
    public AudioClip snurtleLovedClip;
    public AudioClip snurtleAngryClip;
    public AudioClip snurtleAttackClip;
    public AudioClip snurtleResolvedClip;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        hitbox.Initialize();
        hitbox?.SetUniformDamage(targets, animalDamage * damageMultiplier);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    protected override void OnEmotionChanged(Emotion newEmotion)
    {
        base.OnEmotionChanged(newEmotion);

        switch (newEmotion)
        {
            case Emotion.LOVE:
                snurtleAudioSource.PlayOneShot(snurtleLovedClip);
                break;
            case Emotion.ANGER:
                snurtleAudioSource.PlayOneShot(snurtleAngryClip);
                break;
            case Emotion.DEFENCE:
                snurtleAudioSource.PlayOneShot(snurtleResolvedClip);
                break;
        }
    }

    public override void Attack()
    {
        hitbox?.SetUniformDamage(targets, animalDamage * damageMultiplier);
        StartCoroutine(SnurtleAttack());
    }

    IEnumerator SnurtleAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        snurtleAudioSource.PlayOneShot(snurtleAttackClip);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);
    }
}
