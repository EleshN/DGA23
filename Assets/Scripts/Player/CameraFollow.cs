using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform PlayerTrans;
    [SerializeField] Vector3 offset = new Vector3(0f,5f,-5f);

    // Start is called before the first frame update
    void Start()
    {
        PlayerTrans = GameManager.Instance.PlayerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  PlayerTrans.position + offset;
    }
}
