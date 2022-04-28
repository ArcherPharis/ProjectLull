using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] GameObject hitEffect;
    [Range(0, -5)]
    [SerializeField] float gravityDrop;
    float speed = 120f;
    public float damage;
    bool collided = false;
    Vector3 offset;
    public float localAccuracy;



    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (localAccuracy != 100)
        {
            localAccuracy = (1f - (localAccuracy / 100)) * 3f;//equation works for now, improve in future for better variance
            Vector3 newDir = Quaternion.Euler(Random.Range(-localAccuracy, localAccuracy), Random.Range(-localAccuracy, localAccuracy), 0) * transform.forward;
            rigidBody.velocity = newDir * speed;

        }
        else
        {
            Vector3 newDir = Quaternion.Euler(0, 0, 0) * transform.forward;
            rigidBody.velocity = newDir * speed;
        }


        Physics.gravity = new Vector3(0, gravityDrop, 0);
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    private void FixedUpdate()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        
        Destroy(gameObject);
    }

    public void SetBulletAccuracy(float accuracy)
    {
        localAccuracy = accuracy;
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
            }

            var impactEffect = Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(impactEffect, 2f);

            Destroy(gameObject);
        }
    }


}
