using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    //Put this on an object to allow it to destroy itself with an animation event
    public void destroyMe() {
        Destroy(gameObject);
    }
}
