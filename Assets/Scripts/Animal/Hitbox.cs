using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitboxDamage 
{
    public Tag tag;
    public float damage;
}

public class Hitbox : MonoBehaviour
{

    /// <summary>
    /// a map from game object category to the damage it receives on impact with this hitbox
    /// </summary>
    protected Dictionary<string, float> damageMap;

    [Tooltip("the transform of the entity inflicting the damage")]
    /// <summary>
    /// the transform of the entity inflicting the damage
    /// </summary>
    public Transform attackerTransform;


    public void Initialize()
    {
        damageMap = new Dictionary<string, float>();
    }


    /// <summary>
    /// assigns value of damage for all given targets
    /// </summary>
    /// <param name="gameTags"></param>
    /// <param name="damage"></param>
    public void SetUniformDamage(Tag[] gameTags, float damage)
    {
        foreach (Tag tag in gameTags)
        {
            damageMap[tag.ToString()] = damage;
        }
    }

    /// <summary>
    /// assigns value of damage for given category of gameobjects
    /// </summary>
    /// <param name="gameTag"></param>
    /// <param name="damage"></param>
    public void SetDamage(Tag gameTag, float damage)
    {
        damageMap[gameTag.ToString()] = damage;
    }

    /// <summary>
    /// assigns value of damage for given collection of pairs (tag, damage)
    /// </summary>
    /// <param name="damageValues"></param>
    public void SetDamage(HitboxDamage[] damageValues)
    {
        foreach (HitboxDamage hd in damageValues){
            this.SetDamage(hd.tag, hd.damage);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (damageMap.TryGetValue(other.tag, out float damage))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage, attackerTransform);
            gameObject.SetActive(false);
        }
    }

}
