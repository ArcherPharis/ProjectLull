using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damagable
{
    Inventory damageTakenFromPlayerBullet;
    private void Awake()
    {
        damageTakenFromPlayerBullet = GameObject.Find("Player").GetComponent<Inventory>();//this won't work because the
        //player can switch weapons
    }
    public override void Die()
    {
        base.Die();
        Debug.Log("hello, I'm from the derived class, ready to do more specific tasks");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Debug.Log(collision.gameObject.name + " hit me.");
        }
    }


}
