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
    int deathIndex;
    public float speed;
    public float rotateSpeed;
    private void Awake()
    {
        animationSpeed = Animator.StringToHash("Speed");
        deathIndex = Animator.StringToHash("AnimIndex");
    }

    private void Update()
    {
        
    }
    public override void Die()
    {
        base.Die();
        Destroy(gameObject, 5f);
        animator.SetFloat(deathIndex, Random.Range(0, 2));
        animator.SetTrigger("Die");
        
    }

    public void ChangeMovementSpeed(float desiredValue, float dampTime)
    {
        animator.SetFloat(animationSpeed, desiredValue, dampTime, Time.deltaTime);
    }
    public void Attack()
    {
        player.DealDamage(meleeDamageAmount);
    }




}
