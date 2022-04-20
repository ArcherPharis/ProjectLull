using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] GameObject hitEffect;
    float speed = 90f;
    public float damage;
    bool collided = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        rigidBody.velocity = transform.forward * speed;
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "PlayerBullet" && collision.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            if (collision.gameObject.GetComponent<Damagable>())
            {
                Damagable damagable = collision.gameObject.GetComponent<Damagable>();
                damagable.DealDamage(damage);
                damagable.Die();
            }

            var impactEffect = Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(impactEffect, 2f);

            Destroy(gameObject);
        }
    }


}
