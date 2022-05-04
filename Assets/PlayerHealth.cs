using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Player player;
    float rate = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_RemovedSegments",Mathf.Lerp(material.GetFloat("_RemovedSegments"),material.GetFloat("_SegmentCount") - player.Health, Time.deltaTime * 5f));
    }
}
