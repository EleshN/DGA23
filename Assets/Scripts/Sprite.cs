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
    public GameObject mainCam;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        if (agent) {
            agent.updateRotation = false;
        }
        if (!GameManager.Instance.isDebug) {
            foreach (MeshRenderer m in Meshes)
            {
                if (m != null){
                    m.enabled = false;
                }
            }
        }
        if (!mainCam) {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        }
        
        transform.Rotate(Vector3.up, 45);
        Vector3 feetPosition = transform.localPosition;
        float y = feetPosition.y;
        feetPosition.y = 0;
        transform.Translate(new Vector3(0, -y, 0));
        transform.Rotate(Vector3.right, 45);
        transform.Translate(new Vector3(0, y, 0));
        // print(transform.parent.name + " " + transform.position + " " + transform.rotation);
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //transform.forward = mainCam.transform.forward;
    }
}
