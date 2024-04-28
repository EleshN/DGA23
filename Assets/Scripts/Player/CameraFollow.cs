using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Assign to a transform if camera should follow it.")]
    [SerializeField] Transform PlayerTrans;
    [SerializeField] Vector3 offset = new Vector3(0f,5f,-5f);

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerTrans == null){
            // use the player found by the GameManager for typical levels, otherwise use assigned PlayerTransform
            if (GameManager.Instance != null){
                PlayerTrans = GameManager.Instance.PlayerTransform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  PlayerTrans.position + offset;
    }
}
