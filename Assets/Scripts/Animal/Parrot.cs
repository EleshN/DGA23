using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : Animal
{
    public override void AngerTarget()
    {
        throw new System.NotImplementedException();
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void LoveTarget()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Animal")) {
            collision.gameObject.GetComponent<Animal>().SetEmotion(currEmotion);
        }
    }

}
