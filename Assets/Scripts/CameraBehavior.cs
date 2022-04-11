using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 1.2f, -2.6f);
    [SerializeField] Vector3 cameraPos = new Vector3(0, 0, 0);
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if(target == null)
        {
            target = GameObject.Find("Player").transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = target.TransformPoint(cameraOffset);
        transform.LookAt(target);
    }
}
