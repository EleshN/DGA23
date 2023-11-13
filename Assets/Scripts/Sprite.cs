using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sprite : MonoBehaviour
{
    [Header("Other Visuals to disable")]
    [SerializeField]
    List<MeshRenderer> Meshes;
    [SerializeField]
    NavMeshAgent agent;

    //The main camera
    [SerializeField]
    GameObject mainCam;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        if (agent) {
            agent.updateRotation = false;
        }
        if (!GameManager.Instance.isDebug) {
            foreach (MeshRenderer m in Meshes)
            {
                m.enabled = false;
            }
        }
        if (!mainCam) {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        transform.forward = mainCam.transform.forward;
    }
}
