using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damagable
{
    Inventory damageTakenFromPlayerBullet;
    [SerializeField] Player player;
    [SerializeField] float meleeDamageAmount;
    [SerializeField] Animator animator;
    int animationSpeed;
    public float speed;
    public float rotateSpeed;
    private void Awake()
    {
        animationSpeed = Animator.StringToHash("Speed");
    }
    public override void Die()
    {
        base.Die();
        Debug.Log("hello, I'm from the derived class, ready to do more specific tasks");
    }

    public void ChangeMovementSpeed(float desiredValue, float dampTime)
    {
        animator.SetFloat(animationSpeed, desiredValue, dampTime, Time.deltaTime);
    }
    public void Attack()
    {
        player.DealDamage(meleeDamageAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Debug.Log(collision.gameObject.name + " hit me.");
        }
    }


}
