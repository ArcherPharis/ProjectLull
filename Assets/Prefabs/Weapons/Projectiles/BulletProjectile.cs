using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] Transform hitGreen;
    [SerializeField] Transform hitRed;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        float speed = 40f;
        rigidBody.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Damagable>() != null)
        {
            Debug.Log("I'm damagable");
            Instantiate(hitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("I'm not.");
            Instantiate(hitRed, transform.position, Quaternion.identity);
        }
        Debug.Log(other.name);
        Destroy(gameObject);
    }
}
